using System;
using System.Collections.Generic;

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
        internal FeatureFilter(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public FeatureFilter() { }

        /// <summary>
        /// The feature name to be evaluated.
        /// </summary>
        /// <remarks>
        /// The feature name are compared case insensitively with the name on the store.
        /// </remarks>
        public string Name { get; set; }
    }
}
