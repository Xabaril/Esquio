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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the product to create.</param>
        public Product(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
