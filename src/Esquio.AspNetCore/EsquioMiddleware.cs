using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore
{

    //TODO: Create default json serialization settings, try PathString instead string ,remove aspnet core dependencies or check this with core 3
    public class EsquioMiddleware
    {
        private TemplateMatcher _requestPathMatcher;
        private readonly RequestDelegate _next;
        private readonly string _path;

        public EsquioMiddleware(
            RequestDelegate next,
            string path)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _path = path;

            _requestPathMatcher = new TemplateMatcher(
                TemplateParser.Parse(path),
                new RouteValueDictionary());
        }

        public async Task Invoke(HttpContext context, IFeatureService featuresService)
        {
            if (!IsEsquioRequest(context.Request))
            {
                await _next(context);

                return;
            }

            var statusCode = StatusCodes.Status200OK;

            var featureName = context.Request.Query["featureName"];
            var json = String.Empty;

            try
            {
                //TODO: Add aaplication id
                var isEnabled = await featuresService.IsEnabledAsync("",featureName);
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

        private bool IsEsquioRequest(HttpRequest request)
        {
            return request.Method == HttpMethods.Get &&
                   _requestPathMatcher.TryMatch(request.Path, new RouteValueDictionary()) &&
                   request.Query.ContainsKey("featureName");
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
