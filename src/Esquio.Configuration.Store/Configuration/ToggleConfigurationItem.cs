using System.Collections.Generic;

namespace Esquio.Configuration.Store.Configuration
{
    internal class ToggleConfiguration
    {
        public string Type { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}
