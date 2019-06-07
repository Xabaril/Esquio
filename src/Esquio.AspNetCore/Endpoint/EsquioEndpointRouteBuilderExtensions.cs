using Esquio.AspNetCore.Endpoint;
using Microsoft.AspNetCore.Routing;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEndpointRouteBuilder"/>
    /// </summary>
    public static class EsquioEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Map a new endpoint that can  be used to get the activation state of any configured feature.
        /// </summary>
        /// <param name="endpoints"><see cref="IEndpointRouteBuilder"/></param>
        /// <param name="pattern">The uri pattern for this endpoint.</param>
        /// <returns>A <see cref="IEndpointRouteBuilder"/> to continue configuring this new endpoint.</returns>
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
    }
}