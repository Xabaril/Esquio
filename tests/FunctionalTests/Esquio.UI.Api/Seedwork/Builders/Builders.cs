namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public static class Builders
    {
        public static IdentityBuilder Identity()
        {
            return new IdentityBuilder();
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
    }
}
