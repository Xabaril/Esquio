using Esquio.Model;

namespace Esquio.Configuration.Store.Configuration
{
    internal class ProductConfiguration
    {
        public string Name { get; set; }

        public FeatureConfiguration[] Features { get; set; } = new FeatureConfiguration[0];

        internal Product To()
        {
            return new Product(Name);
        }
    }
}
