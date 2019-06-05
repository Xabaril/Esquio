using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoint
{
    public class EsquioMiddleware
    {
        const string FEATURENAME_QUERY_PARAMETER_NAME = "featureName";
        const string PRODUCTNAME_QUERY_PARAMETER_NAME = "productName";

        private readonly RequestDelegate _next;

        public EsquioMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task Invoke(HttpContext context, IFeatureService featureService)
        {
            var statusCode = StatusCodes.Status200OK;

            var featureName = context.Request.Query[FEATURENAME_QUERY_PARAMETER_NAME];
            var productName = context.Request.Query[PRODUCTNAME_QUERY_PARAMETER_NAME];
            var json = String.Empty;

            try
            {
                var isEnabled = await featureService.IsEnabledAsync(featureName, productName);
                var data = new { isEnabled };
                json = JsonConvert.SerializeObject(data);
            }
            catch (ArgumentException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }

            await WriteResponseAsync(
                context,
                json,
                "application/json",
                statusCode);
        }

        private Task WriteResponseAsync(
           HttpContext context,
           string content,
           string contentType,
           int statusCode)
        {
            context.Response.Headers["Content-Type"] = new[] { contentType };
            context.Response.Headers["Cache-Control"] = new[] { "no-cache, no-store, must-revalidate" };
            context.Response.Headers["Pragma"] = new[] { "no-cache" };
            context.Response.Headers["Expires"] = new[] { "0" };
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(content);
        }
    }
}
