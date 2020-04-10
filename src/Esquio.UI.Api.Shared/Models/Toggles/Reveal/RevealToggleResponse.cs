using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models.Toggles.Reveal
{
    public class RevealToggleResponse
    {
        public string Type { get; set; }

        public List<RevealToggleParameterResponse> Parameters { get; set; }
    }

    public class RevealToggleParameterResponse
    {
        public string Name { get; set; }

        public string ClrType { get; set; }

        public string Description { get; set; }
    }
}
