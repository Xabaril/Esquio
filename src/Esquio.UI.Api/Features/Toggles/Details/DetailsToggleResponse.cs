using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Toggles.Details
{
    public class DetailsToggleResponse
    {
        public string TypeName { get; set; }

        public Dictionary<string,string> Parameters { get; set; }
    }
}
