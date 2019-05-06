using System;
using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class FeatureEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ToggleEntity> Toggles { get; set; } = new List<ToggleEntity>();
        public int ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }

        public Feature To()
        {
            return new Feature(Name, Description, CreatedOn, Enabled);
        }
    }
}
