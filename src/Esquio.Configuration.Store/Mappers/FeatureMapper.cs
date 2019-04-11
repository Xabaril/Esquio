namespace Esquio.Configuration.Store.Entities
{
    public static class FeatureMapper
    {
        public static Esquio.Feature To(this Feature feature)
        {
            return new Esquio.Feature(feature.Name, feature.CreatedOn, feature.Enabled);
        }
    }
}
