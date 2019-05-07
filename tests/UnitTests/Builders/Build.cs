using Esquio.Abstractions;

namespace UnitTests.Builders
{
    public static class Build
    {
        public static FeatureBuilder Feature(string name)
        {
            return new FeatureBuilder(name);
        }

        public static ToggleBuilder Toggle<TToggle>()
            where TToggle : IToggle
        {
            return new ToggleBuilder(typeof(TToggle).FullName);
        }
    }
}
