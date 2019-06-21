using Esquio.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ParameterEntityBuilder
    {
        private string _name = "Esquio.Toggles.OnToggle";
        private string _value = string.Empty;

        private int _featureId = 1;

        public ParameterEntityBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ParameterEntityBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }
      
        public ParameterEntity Build()
        {
            return new ParameterEntity(_featureId, _name, _value);
        }
    }
}
