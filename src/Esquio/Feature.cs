using System;

namespace Esquio
{
    class Feature
    {
        public string Name
        {
            get; private set;
        }
        public bool Enabled
        {
            get; private set;
        }
        public DateTime CreatedOn
        {
            get;
            private set;
        }
        public Feature(string name, DateTime createdOn = default, bool enabled = false)
        {
            Name = name;
            CreatedOn = createdOn == default ? DateTime.UtcNow : createdOn;
            Enabled = enabled;
        }
        public static Feature CreateEnabled(string name) => new Feature(name, DateTime.UtcNow, enabled: true);
        public static Feature CreatedDisabled(string name) => new Feature(name, DateTime.UtcNow, enabled: false);
    }
}
