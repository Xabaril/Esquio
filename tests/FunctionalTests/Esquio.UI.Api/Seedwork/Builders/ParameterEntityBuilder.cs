using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ParameterEntityBuilder
    {
        private string _name = "Esquio.Toggles.FromToToggle";
        private string _value = string.Empty;
        private string _ringName;
        private int _toggleId = 1;
        

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

        public ParameterEntityBuilder WithRingName(string deploymentName)
        {
            _ringName = deploymentName;
            return this;
        }
        public ParameterEntityBuilder WithToggle(ToggleEntity toggle)
        {
            _toggleId = toggle.Id;
            return this;
        }


        public ParameterEntity Build()
        {
            return new ParameterEntity(_toggleId, _ringName, _name, _value);
        }
    }
}
