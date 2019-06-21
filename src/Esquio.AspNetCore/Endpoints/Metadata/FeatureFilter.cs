using System;

namespace Esquio.AspNetCore.Endpoints.Metadata
{
    /// <summary>
    /// Use this attribute to add specified metadata to endpoints. This metadata is used o filter if the endpoint can be executed 
    /// depending on feature(s) state. If the configured feature is enabled this endpoint is executed, if not,
    /// by default a NotFound result is obtained. You can modify the default action using
    /// the extension method AddEndpointFallback in <see cref="IEsquioBuilder"/> interface when register Esquio services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class FeatureFilter
       : Attribute, IFeatureFilterMetadata
    {
        internal FeatureFilter(string names, string productName = null)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
            ProductName = productName;
        }

        public FeatureFilter() { }

        /// <summary>
        /// A coma separated list of features names to be evaluated.
        /// </summary>
        /// <remarks>
        /// The feature name are compared case insensitively with the name on the store.
        /// </remarks>
        public string Names { get; set; }

        /// <summary>
        /// The product name when the features are configured. If null a default product is used.
        /// </summary>
        public string ProductName { get; set; }
    }
}
