using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class ParameterEntity
    {
        public int Id { get; set; }
        public int ToggleEntityId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public ParameterEntity(int toggleEntityId, string name, string value)
        {
            ToggleEntityId = toggleEntityId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
