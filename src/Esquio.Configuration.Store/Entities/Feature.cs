using System;

namespace Esquio.Configuration.Store.Entities
{
    public class Feature
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public Toggle[] Toggles { get; set; }
    }
}
