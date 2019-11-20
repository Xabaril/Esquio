using System;
using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class ToggleEntity : IAuditable
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public FeatureEntity FeatureEntity { get; set; }

        public int FeatureEntityId { get; set; }

        public ICollection<ParameterEntity> Parameters { get; set; }

        public ToggleEntity(int featureEntityId, string type)
        {
            FeatureEntityId = featureEntityId;
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Parameters = new List<ParameterEntity>();
        }
    }
}
