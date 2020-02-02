using System;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
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
