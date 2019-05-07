using Esquio.Model;
using System.Collections.Generic;

namespace UnitTests.Builders
{
    public class ToggleBuilder
    {
        private Toggle _toggle;

        public ToggleBuilder(string type)
        {
            _toggle = new Toggle(type);
        }

        public ToggleBuilder AddOneParameter(string name, object value)
        {
            _toggle.AddParameter(new Parameter(name, value));
            return this;
        }

        public Toggle Build()
        {
            return _toggle;
        }
    }
}
