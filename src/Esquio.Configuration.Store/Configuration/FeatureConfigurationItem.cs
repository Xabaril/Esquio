using System;

namespace Esquio.Configuration.Store.Configuration
{
    internal class FeatureConfiguration
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool IsPreview { get; set; }
        public ToggleConfiguration[] Toggles { get; set; }
        public Feature To()
        {
            return new Feature(Name, Description,DateTime.UtcNow, Enabled);
        }
    }
}
