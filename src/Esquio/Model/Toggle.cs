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
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

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

            foreach (var item in _parameters)
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
            foreach (var item in parameters)
            {
                if (_parameters.ContainsKey(item.Name))
                {
                    _parameters[item.Name] = item.Value;
                }
                else
                {
                    _parameters.Add(item.Name, item.Value);
                }
            }
        }

        internal IEnumerable<Parameter> GetParameters()
        {
            return _parameters.Select(p => new Parameter(p.Key, p.Value));
        }
    }
}
