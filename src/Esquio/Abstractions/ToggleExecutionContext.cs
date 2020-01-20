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

        public string RingName { get; private set; }

        public IDictionary<string,object> Data { get; private set; }

        internal ToggleExecutionContext(string featureName, string productName, string ringName, IDictionary<string,object> data)
        {
            FeatureName = featureName ?? throw new ArgumentNullException(nameof(featureName));
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            RingName = ringName ?? throw new ArgumentNullException(nameof(ringName));
            Data = data ?? new Dictionary<string, object>();
        }

        public static ToggleExecutionContext FromToggle(string featureName, string productName, string ringName, Toggle toggle)
        {
            return new ToggleExecutionContext(
                featureName,
                productName,
                ringName,
                toggle.GetParameters().ToDictionary(tp => tp.Name, tp => tp.Value));
        }
    }
}
