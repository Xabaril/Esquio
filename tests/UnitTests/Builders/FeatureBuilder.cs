using Esquio.Model;

namespace UnitTests.Builders
{
    public class FeatureBuilder
    {
        private readonly Feature feature;

        public FeatureBuilder(string name)
        {
            feature = new Feature(name, name);
        }

        public FeatureBuilder AddOne(Toggle toggle)
        {
            feature.AddToggle(toggle);
            return this;
        }

        public FeatureBuilder Enabled()
        {
            feature.Enabled();
            return this;
        }
        public FeatureBuilder Disabled()
        {
            feature.Disabled();
            return this;
        }

        public FeatureBuilder Preview()
        {
            feature.MarkAsPreview();
            return this;
        }

        public Feature Build()
        {
            return feature;
        }
    }
}
