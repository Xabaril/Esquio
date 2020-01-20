using System;
using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class ProductEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<FeatureEntity> Features { get; set; }

        public ICollection<RingEntity> Rings { get; set; }

        public ProductEntity(string name, string description = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;

            Features = new List<FeatureEntity>();
            Rings = new List<RingEntity>();
        }
    }
}
