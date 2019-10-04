using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Esquio.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Endpoints
{
    internal class EsquioMiddleware
    {
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        const string FEATURENAME_QUERY_PARAMETER_NAME = "featureName";
        const string PRODUCTNAME_QUERY_PARAMETER_NAME = "productName";
        const string DEFAULT_MIME_TYPE = MediaTypeNames.Application.Json;

        private readonly RequestDelegate _next;

        public EsquioMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task Invoke(HttpContext context, IFeatureService featureService, EsquioAspNetCoreDiagnostics diagnostics)
        {
            var evaluationsResponse = new List<EvaluationResponse>();

            var names = context.Request
                .Query[FEATURENAME_QUERY_PARAMETER_NAME];

            var productName = context.Request
                .Query[PRODUCTNAME_QUERY_PARAMETER_NAME]
                .LastOrDefault();

            foreach (var featureName in names)
            {
                try
                {
                    diagnostics.EsquioMiddlewareEvaluatingFeature(featureName, productName);

                    var isEnabled = await featureService
                        .IsEnabledAsync(featureName, productName, context?.RequestAborted ?? CancellationToken.None);

                    evaluationsResponse.Add(new EvaluationResponse()
                    {
                        Name = featureName,
                        Enabled = isEnabled
                    });
                }
                catch (Exception exception)
                {
                    diagnostics.EsquioMiddlewareThrow(featureName, productName, exception);

                    await WriteError(context, featureName, productName);

                    return;
                }
            }

            diagnostics.EsquioMiddlewareSuccess();

            await WriteResponse(context,evaluationsResponse);
        }

        private async Task WriteResponse(HttpContext currentContext, IEnumerable<EvaluationResponse> response)
        {
            await WriteAsync(
                currentContext,
                JsonSerializer.Serialize(response, options: _serializerOptions),
                DEFAULT_MIME_TYPE,
                StatusCodes.Status200OK);
        }

        private async Task WriteError(HttpContext currentContext, string featureName, string productName)
        {
            await WriteAsync(
                currentContext,
                JsonSerializer.Serialize(EvaluationError.Default(featureName, productName), options: _serializerOptions),
                DEFAULT_MIME_TYPE,
                StatusCodes.Status500InternalServerError);
        }

        private async Task WriteAsync(
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

        private class EvaluationResponse
        {
            public bool Enabled { get; set; }

            public string Name { get; set; }
        }

        private class EvaluationError
        {
            public string Message { get; set; }

            private EvaluationError() { }

            public static EvaluationError Default(string featureName, string productName)
            {
                return new EvaluationError()
                {
                    Message = $"{nameof(OnErrorBehavior)} behavior for Esquio is configured to {nameof(OnErrorBehavior.Throw)} and middleware throw when check the state for {featureName} on product {productName ?? "default product"}."
                    + $"You can modify this behavior using {nameof(EsquioOptions.ConfigureOnErrorBehavior)} method on AddEsquio options."
                };
            }
        }
    }
}
