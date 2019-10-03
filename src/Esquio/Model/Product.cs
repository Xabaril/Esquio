using System;

namespace Esquio.Model
{
    /// <summary>
    /// Represent the internal model for represent Product on Esquio library. Typically
    /// this class is used for configuration providers to mapping provider model to Esquio.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Get the product name.
        /// </summary>
        public string Name { get; }

        public Product(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
