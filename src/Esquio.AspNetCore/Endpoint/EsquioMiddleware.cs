using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            var response = new List<EsquioResponse>();

            var names = context.Request
                .Query[FEATURENAME_QUERY_PARAMETER_NAME];

            var productName = context.Request
                .Query[PRODUCTNAME_QUERY_PARAMETER_NAME]
                .LastOrDefault();

            try
            {
                foreach (var featureName in names)
                {
                    var isEnabled = await featureService
                        .IsEnabledAsync(featureName, productName, context?.RequestAborted ?? CancellationToken.None);

                    response.Add(new EsquioResponse()
                    {
                        Name = featureName,
                        Enabled = isEnabled
                    });
                }

                await WriteResponseAsync(
                    context,
                    JsonConvert.SerializeObject(response),
                    "application/json",
                    StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                await WriteResponseAsync(
                   context,
                   string.Empty,
                   "application/json",
                   StatusCodes.Status500InternalServerError);
            }
        }

        private async Task WriteResponseAsync(
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

            await context.Response.WriteAsync(content);
        }

        private class EsquioResponse
        {
            public bool Enabled { get; set; }

            public string Name { get; set; }
        }
    }
}
