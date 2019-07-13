using System;
using System.Collections.Generic;
using System.Text;

namespace Esquio.UI.Api.Features.Flags.DetailsExtended
{
    public class DetailsExtendedFlagResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public string ProductName { get; set; }

        public List<ToggleDetail> Toggles { get; set; }
    }

    public class ToggleDetail {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
