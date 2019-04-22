using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore
{
    public class EsquioMiddleware
    {
        const string FEATURENAME_QUERY_PARAMETER_NAME = "featureName";
        const string APPLICATIONNAME_QUERY_PARAMETER_NAME = "applicationName";

        private readonly RequestDelegate _next;

        public EsquioMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task Invoke(HttpContext context, IFeatureService featureService)
        {
            var statusCode = StatusCodes.Status200OK;

            var featureName = context.Request.Query[FEATURENAME_QUERY_PARAMETER_NAME];
            var applicationName = context.Request.Query[APPLICATIONNAME_QUERY_PARAMETER_NAME];
            var json = String.Empty;

            try
            {
                var isEnabled = await featureService.IsEnabledAsync(applicationName, featureName);
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
