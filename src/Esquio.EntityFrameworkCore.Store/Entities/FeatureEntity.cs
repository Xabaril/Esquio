using Esquio.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class FeatureEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool IsPreview { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ToggleEntity> Toggles { get; set; } = new List<ToggleEntity>();
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }

        public Feature To()
        {
            var feature = new Feature(Name, Description, CreatedOn);

            if (IsPreview)
            {
                feature.MarkAsPreview();
            }

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
                toggle.AddParameters(toggleConfiguration.Parameters.Select(p => new Parameter(p.Name, p.Value)));
                feature.AddToggle(toggle);
            }

            return feature;
        }

        public void CopyFrom(Feature feature)
        {
            throw new NotImplementedException();
        }
    }
}
