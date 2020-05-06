using Esquio.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.Abstractions
{
    public sealed class ToggleExecutionContext
    {
        public string FeatureName { get; private set; }

        public string ProductName { get; private set; }

        public string DeploymentName { get; private set; }

        public IDictionary<string,object> Data { get; private set; }

        internal ToggleExecutionContext(string featureName, string productName, string deploymentName, IDictionary<string,object> data)
        {
            FeatureName = featureName ?? throw new ArgumentNullException(nameof(featureName));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            DeploymentName = deploymentName ?? throw new ArgumentNullException(nameof(deploymentName));
            Data = data ?? new Dictionary<string, object>();
        }

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
