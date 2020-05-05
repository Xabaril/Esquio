using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Features.State
{
    public class StateFeatureResponse
    {
        public List<StateFeatureResponseDetail> States { get; set; }
    }
    public class StateFeatureResponseDetail
    {
        public string RingName { get; set; }

        public bool Default { get; set; }

        public bool Enabled { get; set; }
    }
}
