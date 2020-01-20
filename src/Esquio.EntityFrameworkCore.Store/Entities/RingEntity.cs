using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class RingEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductEntityId { get; set; }

        public ProductEntity Product { get; set; }

        public RingEntity(int productEntityId, string name)
        {
            ProductEntityId = productEntityId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
