using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Esquio.Model
{
    public class Toggle
    {
        private readonly List<Parameter> _parameters = new List<Parameter>();

        public Toggle(string type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public string Type { get; }

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

        public TValue GetToggleParameterValue<TValue>(string name)
        {
            var parameter = _parameters.SingleOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (parameter != null)
            {
                return (TValue)parameter.Value;
            }

            return default;
        }
       
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
