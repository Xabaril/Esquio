using System;
using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class FeatureEntity : IAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ProductEntityId { get; set; }

        public bool Enabled { get; set; }

        public bool Archived { get; set; }

        public ProductEntity ProductEntity { get; set; }

        public ICollection<FeatureTagEntity> FeatureTags { get; set; }

        public ICollection<ToggleEntity> Toggles { get; set; }

        public FeatureEntity(int productEntityId, string name, bool enabled = false, bool archived = false, string description = null)
        {
            ProductEntityId = productEntityId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Enabled = enabled;
            Archived = archived;
            Description = description;
            Toggles = new List<ToggleEntity>();
            FeatureTags = new List<FeatureTagEntity>();
        }
    }
}
