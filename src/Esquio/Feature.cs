using System;

namespace Esquio
{
    public class Feature
    {
        public string Name
        {
            get; private set;
        }
        public string Description
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
        public Feature(string name, string description, DateTime createdOn = default, bool enabled = false)
        {
            Ensure.That<ArgumentException>(!string.IsNullOrWhiteSpace(name), nameof(name));
            Ensure.That<ArgumentException>(!string.IsNullOrWhiteSpace(description), nameof(description));

            Name = name;
            Description = description;
            CreatedOn = createdOn == default ? DateTime.UtcNow : createdOn;
            Enabled = enabled;
        }
        public static Feature CreateEnabled(string name, string description) => new Feature(name, description, enabled: true);
        public static Feature CreatedDisabled(string name, string description) => new Feature(name, description, enabled: false);
    }
}
