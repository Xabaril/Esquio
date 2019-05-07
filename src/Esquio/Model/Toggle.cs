using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.Model
{
    public class Toggle
    {
        private readonly List<Parameter> _parameters = new List<Parameter>();

        public Toggle(string type)
        {
            Ensure.Argument.NotNullOrEmpty(type, nameof(type));

            Type = type;
        }
        public string Type { get; }

        public void AddParameter(Parameter parameter)
        {
            _parameters.Add(parameter);
        }
        public void AddParameters(IEnumerable<Parameter> parameters)
        {
            _parameters.AddRange(parameters);
        }
        public T GetParameterValue<T>(string name)
        {
            var parameter = _parameters.SingleOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (parameter != null)
            {
                return (T)parameter.Value;
            }

            return default;
        }
    }
}
