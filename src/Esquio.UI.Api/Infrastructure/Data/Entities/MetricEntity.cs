using System;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public class MetricEntity
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string RingName { get; set; }

        public string Kind { get; set; }

        public DateTime DateTime { get; set; }
    }
}
