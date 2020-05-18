using Esquio.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.Abstractions
{
    /// <summary>
    /// The Toogle execution context used on toggle evaluations.
    /// </summary>
    public sealed class ToggleExecutionContext
    {
        /// <summary>
        /// The feature name.
        /// </summary>
        public string FeatureName { get; private set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public string ProductName { get; private set; }

        /// <summary>
        /// The deployment name.
        /// </summary>
        public string DeploymentName { get; private set; }

        /// <summary>
        /// The toggle execution context data.
        /// </summary>
        public IDictionary<string,object> Data { get; private set; }

        internal ToggleExecutionContext(string featureName, string productName, string deploymentName, IDictionary<string,object> data)
        {
            FeatureName = featureName ?? throw new ArgumentNullException(nameof(featureName));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            DeploymentName = deploymentName ?? throw new ArgumentNullException(nameof(deploymentName));
            Data = data ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Create a new toggle execution context.
        /// </summary>
        /// <param name="featureName">The feature name.</param>
        /// <param name="productName">The product name.</param>
        /// <param name="deploymentName">The deployment name.</param>
        /// <param name="toggle">The toggle to evaluate.</param>
        /// <returns>A new <see cref="ToggleExecutionContext"/></returns>
        public static ToggleExecutionContext FromToggle(string featureName, string productName, string deploymentName, Toggle toggle)
        {
            return new ToggleExecutionContext(
                featureName,
                productName,
                deploymentName,
                toggle.GetParameters().ToDictionary(tp => tp.Name, tp => tp.Value));
        }
    }
}
