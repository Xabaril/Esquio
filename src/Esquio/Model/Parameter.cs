using System;

namespace Esquio.Model
{
    public class Parameter
    {
        public Parameter(string name, object value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Name { get; }
        public object Value { get; }
    }
}