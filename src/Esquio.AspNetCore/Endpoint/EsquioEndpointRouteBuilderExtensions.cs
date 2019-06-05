using Esquio.AspNetCore.Endpoint;
using Esquio.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class EsquioEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapEsquio(this IEndpointRouteBuilder endpoints, string pattern = "esquio")
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<EsquioMiddleware>()
                .Build();

            return endpoints.MapGet(pattern, pipeline);
        }

        public static void MapControllerRouteWithFeatureCondition(
            this IEndpointRouteBuilder endpoints,
            string name,
            string pattern,
            string featureName,
            string productName = null,
            object defaults = null)
        {
            endpoints.MapControllerRoute(
                name,
                pattern,
                defaults: defaults,
                constraints: new { name = new FeatureFilterRouteConstraint(endpoints.ServiceProvider, featureName, productName) });
        }
    }
}