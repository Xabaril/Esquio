using System;
using System.Collections.Generic;
using System.Text;

namespace Esquio.UI.Api.Features.Toggles.Reveal
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
