using Esquio.AspNetCore.Endpoint.Metadata;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// A collection of extensions methods for <see cref="IEndpointConventionBuilder"/>.
    /// </summary>
    public static class EndpointConventionBuilderExtensions
    {
        private static char[] split_characters = new char[] { ',' };

        /// <summary>
        /// Specify a feature check to the endpoint(s).
        /// </summary>
        /// <typeparam name="TBuilder"><see cref="IEndpointConventionBuilder"/></typeparam>
        /// <param name="builder">The endopoint convention builder.</param>
        /// <param name="featureName">A coma separated list of features names to be evaluated.</param>
        /// <param name="productName">The product name to be checked with the <paramref name="featureName"/> parameter.</param>
        /// <returns>The original convention builder to be chained.</returns>
        public static TBuilder RequireFeature<TBuilder>(this TBuilder builder, string featureNames, string productName = null) where TBuilder : IEndpointConventionBuilder
        {
            builder.Add(endpointbuilder =>
            {
                var tokenizer = new StringTokenizer(featureNames, split_characters);

                foreach (var token in tokenizer)
                {
                    if (token.HasValue)
                    {
                        endpointbuilder.Metadata.Add(new FeatureData(token.Value, productName));
                    }
                }
            });

            return builder;
        }
    }
}
