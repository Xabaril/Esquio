using System.Collections.Generic;

namespace Esquio.UI.Api.Scenarios.Toggles.KnownTypes
{
    public class KnownTypesToggleResponse
    {
        public int ScannedAssemblies { get; set; }

        public List<KnownTypesToggleDetailResponse> Toggles { get; set; }
    }

    public class KnownTypesToggleDetailResponse
    {
        public string Assembly { get; set; }

        public string Type { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }
    }
}
