using System;

namespace Esquio.UI.Api.Infrastructure.Metrics
{
    public class ConfigurationRequestMetric
    {
        public string ProductName { get; private set; }

        public string FeatureName { get; private set; }

        public string RingName { get; private set; }

        public string Kind { get; private set; }

        public DateTime RequestedAt { get; private set; }

        private ConfigurationRequestMetric()
        {

        }

        public static ConfigurationRequestMetric FromSuccess(string productName, string featureName, string ringName)
        {
            return new ConfigurationRequestMetric()
            {
                ProductName = productName,
                FeatureName = featureName,
                RingName = ringName,
                RequestedAt = DateTime.UtcNow,
                Kind = "Success"
            };
        }

        public static ConfigurationRequestMetric FromNotFound(string productName, string featureName, string ringName)
        {
            return new ConfigurationRequestMetric()
            {
                ProductName = productName,
                FeatureName = featureName,
                RingName = ringName,
                RequestedAt = DateTime.UtcNow,
                Kind = "NotFound"
            };
        }
    }
}
