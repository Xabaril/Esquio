using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Esquio.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoints
{
    internal class EsquioMiddleware
    {
        const string FEATURENAME_QUERY_PARAMETER_NAME = "featureName";
        const string PRODUCTNAME_QUERY_PARAMETER_NAME = "productName";

        private readonly RequestDelegate _next;

        public EsquioMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task Invoke(HttpContext context, IFeatureService featureService, ILogger<EsquioMiddleware> logger)
        {
            var response = new List<EsquioMiddlewareResponse>();

            var names = context.Request
                .Query[FEATURENAME_QUERY_PARAMETER_NAME];

            var productName = context.Request
                .Query[PRODUCTNAME_QUERY_PARAMETER_NAME]
                .LastOrDefault();

            foreach (var featureName in names)
            {
                try
                {
                    Log.EsquioMiddlewareEvaluatingFeature(logger, featureName, productName);

                    var isEnabled = await featureService
                        .IsEnabledAsync(featureName, productName, context?.RequestAborted ?? CancellationToken.None);

                    response.Add(new EsquioMiddlewareResponse()
                    {
                        Name = featureName,
                        Enabled = isEnabled
                    });
                }
                catch (Exception exception)
                {
                    // only when OnError behavior is configured to Throw!!

                    Log.EsquioMiddlewareThrow(logger, featureName, productName, exception);

                    await WriteResponseAsync(
                       context,
                       JsonConvert.SerializeObject(EsquioMiddlewareError.Default(featureName, productName)),
                       "application/json",
                       StatusCodes.Status500InternalServerError);

                    return;
                }
            }

            Log.EsquioMiddlewareSuccess(logger);

            await WriteResponseAsync(
                context,
                JsonConvert.SerializeObject(response),
                "application/json",
                StatusCodes.Status200OK);
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

        private class EsquioMiddlewareResponse
        {
            public bool Enabled { get; set; }

            public string Name { get; set; }
        }
        private class EsquioMiddlewareError
        {
            public string Message { get; set; }

            private EsquioMiddlewareError() { }

            public static EsquioMiddlewareError Default(string featureName, string productName)
            {
                return new EsquioMiddlewareError()
                {
                    Message = $"{nameof(OnErrorBehavior)} behavior for Esquio is configured to {nameof(OnErrorBehavior.Throw)} and middleware throw when check the state for {featureName} on product {productName ?? "default product"}."
                    + $"You can modify this behavior using {nameof(EsquioOptions.ConfigureOnErrorBehavior)} method on AddEsquio options."
                };
            }
        }
    }
}
