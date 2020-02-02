using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Scenarios.Store.Details
{
    public class DetailsStoreRequest
        : IRequest<DetailsStoreResponse>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string RingName { get; set; }
    }

    public class DetailsStoreResponse
    {
        public string FeatureName { get; set; }

        public bool Enabled { get; set; }

        public Dictionary<string, Dictionary<string, object>> Toggles { get; set; } = new Dictionary<string, Dictionary<string, object>>();
    }
}
