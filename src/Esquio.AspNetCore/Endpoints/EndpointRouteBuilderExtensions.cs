using Esquio.AspNetCore.Endpoints;
using Esquio.AspNetCore.Endpoints.Metadata;
using Microsoft.AspNetCore.Routing;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Provides Esquio extensions methods for <see cref="IEndpointRouteBuilder"/>
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        private static char[] split_characters = new char[] { ',' };

        /// <summary>
        /// Specify a feature check to the endpoint(s).
        /// </summary>
        /// <typeparam name="TBuilder"><see cref="IEndpointConventionBuilder"/></typeparam>
        /// <param name="builder">The endopoint convention builder.</param>
        /// <param name="names">A coma separated list of features names to be evaluated.</param>
        /// <param name="productName">The product name to be checked with the <paramref name="featureName"/> parameter.</param>
        /// <returns>The original convention builder to be chained.</returns>
        public static TBuilder RequireFeature<TBuilder>(this TBuilder builder, string names, string productName = null) where TBuilder : IEndpointConventionBuilder
        {
            builder.Add(endpointbuilder =>
            {
                var metadata = new FeatureFilter(names, productName);

                endpointbuilder.Metadata
                    .Add(metadata);
            });

            return builder;
        }

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