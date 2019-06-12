using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class ApiKeyEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public ApiKeyEntity(string name, string description, string key)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
    }
}
