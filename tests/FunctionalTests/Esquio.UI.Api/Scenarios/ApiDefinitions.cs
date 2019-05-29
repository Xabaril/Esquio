namespace FunctionalTests.Esquio.UI.Api.Scenarios
{
    public static class ApiDefinitions
    {
        public static class V1
        {
            public static class Flags
            {
                public static string Rollout(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flags/{featureId}/rollout";
                }
                public static string Delete(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flags/{featureId}";
                }
                public static string Get(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flags/{featureId}";
                }
                public static string List(int productId)
                {
                    return $"api/v1/product/{productId}/flags";
                }
                public static string List(int productId,int pageIndex,int pageCount)
                {
                    return $"api/v1/product/{productId}/flags?pageIndex={pageIndex}&pageCount={pageCount}";
                }
            }
            public static class Toggles
            {
                public static string Get(int toggleId)
                {
                    return $"api/v1/toggle/{toggleId}";
                }
                public static string Delete(int toggleId)
                {
                    return $"api/v1/toggle/{toggleId}";
                }
                public static string Reveal(int toggleId)
                {
                    return $"api/v1/toggle/{toggleId}/parameters/reveal";
                }
            }
        }
    }
}
