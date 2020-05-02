using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class ParameterEntity : IAuditable
    {
        public int Id { get; set; }

        public int ToggleEntityId { get; set; }

        public ToggleEntity ToggleEntity { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string RingName { get; set; }


        public ParameterEntity(int toggleEntityId, string ringName,string name, string value)
        {
            ToggleEntityId = toggleEntityId;
            RingName = ringName ?? throw new ArgumentNullException(nameof(ringName));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
