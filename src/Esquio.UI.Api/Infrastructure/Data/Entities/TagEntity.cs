using System;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public sealed class TagEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string HexColor { get; set; }

        public TagEntity(string name,string hexColor = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            HexColor = hexColor;
        }
    }
}
