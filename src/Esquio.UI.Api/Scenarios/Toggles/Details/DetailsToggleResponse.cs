using System.Collections.Generic;

namespace Esquio.UI.Api.Scenarios.Toggles.Details
{
    public class DetailsToggleResponse
    {
        public string Type { get; set; }

        public string Assembly { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public List<ParameterDetail> Parameters { get; set; }
    }

    public class ParameterDetail
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
