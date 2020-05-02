using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Configuration.Details
{
    public class DetailsConfigurationRequest
        : IRequest<DetailsConfigurationResponse>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string RingName { get; set; }
    }

    public class DetailsConfigurationResponse
    {
        public string FeatureName { get; set; }

        public bool Enabled { get; set; }

        public Dictionary<string, Dictionary<string, object>> Toggles { get; set; } = new Dictionary<string, Dictionary<string, object>>();
    }
}
