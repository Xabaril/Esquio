namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public static class Builders
    {
        public static HistoryBuilder History()
        {
            return new HistoryBuilder();
        }
        public static IdentityBuilder Identity()
        {
            return new IdentityBuilder();
        }

        public static PermissionsBuilder Permission()
        {
            return new PermissionsBuilder();
        }

        public static ProductEntityBuilder Product()
        {
            return new ProductEntityBuilder();
        }
        
        public static FeatureEntityBuilder Feature()
        {
            return new FeatureEntityBuilder();
        }
        
        public static ToggleEntityBuilder Toggle()
        {
            return new ToggleEntityBuilder();
        }

        public static ParameterEntityBuilder Parameter()
        {
            return new ParameterEntityBuilder();
        }
        public static TagBuilder Tag()
        {
            return new TagBuilder();
        }

        public static FeatureTagBuilder FeatureTag()
        {
            return new FeatureTagBuilder();
        }

        public static ApiKeyBuilder ApiKey()
        {
            return new ApiKeyBuilder();
        }
    }
}
