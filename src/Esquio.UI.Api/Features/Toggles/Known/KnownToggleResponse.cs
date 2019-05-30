using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Toggles.Known
{
    public class KnownToggleResponse
    {
        public int ScannedAssemblies { get; set; }

        public List<KnownToggleDetailResponse> Toggles { get; set; }
    }

    public class KnownToggleDetailResponse
    {
        public string Assembly { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }
}
