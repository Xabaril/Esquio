using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class ToggleEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<ParameterEntity> Parameters { get; set; } = new List<ParameterEntity>();
        public int FeatureId { get; set; }
        public FeatureEntity Feature { get; set; }
    }
}
