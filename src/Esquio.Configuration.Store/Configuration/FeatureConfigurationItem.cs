using Esquio.Model;
using System;
using System.Linq;

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
            var feature = new Feature(Name, Description,DateTime.UtcNow, Enabled);

            foreach (var toggleConfiguration in Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Type);
                toggle.AddParameters(toggleConfiguration.Parameters.Select(p => new Parameter(p.Name, p.Value)));
                feature.AddToggle(toggle);
            }

            return feature;
        }
    }
}
