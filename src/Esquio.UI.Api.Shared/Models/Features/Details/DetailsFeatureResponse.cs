using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Features.Details
{
    public class DetailsFeatureResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public List<ToggleDetail> Toggles { get; set; }
    }

    public class ToggleDetail
    {
        public string Type { get; set; }

        public string FriendlyName { get; set; }

        public List<ParameterDetail> Parameters { get; set; }
    }

    public class ParameterDetail
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
