using System;
using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    internal class FeatureEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ToggleEntity> Toggles { get; set; } = new List<ToggleEntity>();
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }

        public static FeatureEntity New(string name, bool enabled)
        {
            return new FeatureEntity
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                Enabled = enabled
            };
        }

        public Feature To()
        {
            return new Feature(Name, CreatedOn, Enabled);
        }
    }
}
