namespace Esquio.Configuration.Store.Configuration
{
    internal class ProductConfiguration
    {
        public string Name { get; set; }

        public FeatureConfiguration[] Features { get; set; } = new FeatureConfiguration[0];
    }
}
