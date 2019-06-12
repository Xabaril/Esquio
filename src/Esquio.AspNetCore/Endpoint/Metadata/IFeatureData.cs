using System;

namespace Esquio.AspNetCore.Endpoint.Metadata
{
    /// <summary>
    /// Defines the set of data required to apply feature filter to a resource
    /// </summary>
    public interface IFeatureData
    {
        /// <summary>
        /// Gets or sets the feature name used to check if resource is enabled or not.
        /// </summary>
        public string FeatureName { get; }

        /// <summary>
        /// Gets or sets the product name used to check if resource is enabled or not.
        /// </summary>
        public string ProductName { get; }
    }

    internal class FeatureData : Attribute, IFeatureData
    {
        public FeatureData(string featureName, string productName = null)
        {
            FeatureName = featureName ?? throw new ArgumentNullException(nameof(featureName));
            ProductName = productName;
        }

        public string FeatureName { get; private set; }

        public string ProductName { get; private set; }
    }
}
