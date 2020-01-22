using Esquio.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ParameterEntityBuilder
    {
        private string _name = "Esquio.Toggles.FromToToggle";
        private string _value = string.Empty;
        private int _toggleId = 1;
        private int _ringId;

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

        public ParameterEntityBuilder WithRing(RingEntity ring)
        {
            _ringId = ring.Id;
            return this;
        }
        public ParameterEntityBuilder WithRing(ToggleEntity toggle)
        {
            _toggleId = toggle.Id;
            return this;
        }


        public ParameterEntity Build()
        {
            return new ParameterEntity(_toggleId, _ringId,_name, _value);
        }
    }
}
