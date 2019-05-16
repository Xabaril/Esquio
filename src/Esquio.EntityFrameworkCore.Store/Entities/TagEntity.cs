using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TagEntity(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
