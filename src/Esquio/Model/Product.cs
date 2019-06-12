using System;

namespace Esquio.Model
{
    public class Product
    {
        public string Name { get; }
        public string Description { get; }

        public Product(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }
    }
}
