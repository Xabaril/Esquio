using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public string ProductName { get; set; }

        public List<string> Toggles { get; set; }
    }
}
