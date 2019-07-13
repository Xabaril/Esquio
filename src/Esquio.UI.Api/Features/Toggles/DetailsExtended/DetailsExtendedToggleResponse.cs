using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Toggles.DetailsExtended
{
    public class DetailsExtendedToggleResponse
    {
        public string TypeName { get; set; }

        public List<ParameterDetail> Parameters { get; set; }
    }

    public class ParameterDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
