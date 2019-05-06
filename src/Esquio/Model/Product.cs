using System;

namespace Esquio.Model
{
    public class Product
    {
        public string Name { get; }
        public string Description { get; }

        public Product(string name, string description)
        {
            Ensure.That<ArgumentException>(string.IsNullOrWhiteSpace(name));
            Ensure.That<ArgumentException>(string.IsNullOrWhiteSpace(description));
        }
    }
}
