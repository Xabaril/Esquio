using System;

namespace Esquio.Configuration.Store.Configuration
{
    internal class FeatureConfiguration
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public ToggleConfiguration[] Toggles { get; set; }

        public Feature To()
        {
            return new Feature(Name, Description, CreatedOn, Enabled);
        }
    }
}
