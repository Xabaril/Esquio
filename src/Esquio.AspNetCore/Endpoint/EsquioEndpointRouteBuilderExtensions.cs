using Esquio.AspNetCore.Endpoint;
using Microsoft.AspNetCore.Routing;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class EsquioEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapEsquio(this IEndpointRouteBuilder endpoints, string pattern)
        {
            const string DisplayName = "Esquio";

            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<EsquioMiddleware>()
                .Build();

            return endpoints.MapGet(pattern, DisplayName, pipeline);
        }
    }
}