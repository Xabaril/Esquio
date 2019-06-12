using Esquio.EntityFrameworkCore.Store.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ToggleEntityBuilder
    {
        private int _featureId = 1;
        private string _type = "default-type";

        public ToggleEntityBuilder WithFeature(FeatureEntity feature)
        {
            _featureId = feature.Id;
            return this;
        }
        public ToggleEntityBuilder WithType(string type)
        {
            _type = type;
            return this;
        }
        public ToggleEntity Build()
        {
            return new ToggleEntity(_featureId, _type);
        }
    }
}
