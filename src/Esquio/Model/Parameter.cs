using System;

namespace Esquio.Model
{
    /// <summary>
    /// Represent the internal model for represent Parameter on Esquio library. Typically
    /// this class is used for configuration providers to mapping provider model to Esquio.
    /// </summary>
    public class Parameter
    {

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The current value of the parameter.</param>
        public Parameter(string name, object value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Get the parameter name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the parameter value.
        /// </summary>
        public object Value { get; }
    }
}