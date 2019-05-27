using Esquio.Model;
using System.Linq;

namespace Esquio.Configuration.Store.Configuration
{
    internal class FeatureConfiguration
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public ToggleConfiguration[] Toggles { get; set; }
        public Feature To()
        {
            var feature = new Feature(Name);

            if (Enabled)
            {
                feature.Enabled();
            }
            else
            {
                feature.Disabled();
            }

            foreach (var toggleConfiguration in Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Type);
                var configuredParameters = toggleConfiguration.Parameters;

                toggle.AddParameters(configuredParameters.Select(p => new Parameter(p.Key, p.Value)));
                feature.AddToggle(toggle);
            }

            return feature;
        }
    }
}
