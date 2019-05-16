using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using System.Linq;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public static class EntityExtensions
    {
        public static Feature To(this FeatureEntity featureEntity)
        {
            var feature = new Feature(featureEntity.Name);

            if (featureEntity.Enabled)
            {
                feature.Enabled();
            }
            else
            {
                feature.Disabled();
            }

            foreach (var toggleConfiguration in featureEntity.Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Type);
                toggle.AddParameters(toggleConfiguration.Parameters.Select(p => new Parameter(p.Name, p.Value)));
                feature.AddToggle(toggle);
            }

            return feature;
        }
    }
}
