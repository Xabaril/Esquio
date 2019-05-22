using Esquio.Model;

namespace FunctionalTests.Base.Builders
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
            _toggle.AddParameters(new Parameter[]
            {
                new Parameter(name, value)
            });

            return this;
        }

        public Toggle Build()
        {
            return _toggle;
        }
    }
}
