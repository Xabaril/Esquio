using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class ApiKeyEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ValidTo { get; set; }

        public string Key { get; set; }

        public ApiKeyEntity(string name, string key, DateTime validTo)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Key = key ?? throw new ArgumentNullException(nameof(key));
            ValidTo = validTo;
        }
    }
}
