using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class ParameterEntity : IAuditable
    {
        public int Id { get; set; }

        public int ToggleEntityId { get; set; }

        public ToggleEntity ToggleEntity { get; set; }

        public int RingEntityId { get; set; }

        public RingEntity RingEntity { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }


        public ParameterEntity(int toggleEntityId, int ringEntityId, string name, string value)
        {
            ToggleEntityId = toggleEntityId;
            RingEntityId = ringEntityId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
