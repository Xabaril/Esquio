namespace Esquio.Configuration.Store.Configuration
{
    internal class ApplicationConfiguration
    {
        public string Name { get; set; }

        public FeatureConfiguration[] Features { get; set; } = new FeatureConfiguration[0];
    }
}
