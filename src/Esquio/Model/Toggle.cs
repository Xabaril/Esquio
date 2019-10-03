using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Esquio.Model
{
    /// <summary>
    /// Represent the internal model for represent Toggle on Esquio library. Typically
    /// this class is used for configuration providers to mapping provider model to Esquio.
    /// </summary>
    public class Toggle
    {
        private readonly List<Parameter> _parameters = new List<Parameter>();

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="type">The current <see cref="Toggle"/> type.</param>
        public Toggle(string type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Get the current type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Get parameters as new <see cref="ExpandoObject"/>.
        /// </summary>
        /// <returns>The configured parameters as <see cref="ExpandoObject"/>.</returns>
        public dynamic GetData()
        {
            var data = new ExpandoObject();
            var dataDictionary = (IDictionary<string, object>)data;

            foreach(var item in _parameters.ToDictionary(k=>k.Name,k=>k.Value))
            {
                dataDictionary.Add(item);
            }

            return data;
        }

        /// <summary>
        /// Add new collection of parameters.
        /// </summary>
        /// <param name="parameters">The enumerable collection of parameteres to be added.</param>
        public void AddParameters(IEnumerable<Parameter> parameters)
        {
            _parameters.AddRange(parameters);
        }
        
        internal IEnumerable<Parameter> GetParameters()
        {
            return _parameters;
        }
    }
}
