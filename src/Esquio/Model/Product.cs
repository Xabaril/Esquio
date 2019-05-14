using System;

namespace Esquio.Model
{
    public class Product
    {
        public string Name { get; }
        public string Description { get; }

        public Product(string name, string description)
        {
            Ensure.Argument.NotNullOrEmpty(name, nameof(name));
            Name = name;
            Description = description;
        }
    }
}
