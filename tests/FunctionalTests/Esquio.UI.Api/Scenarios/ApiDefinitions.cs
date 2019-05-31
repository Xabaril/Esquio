namespace FunctionalTests.Esquio.UI.Api.Scenarios
{
    public static class ApiDefinitions
    {
        public static class V1
        {
            public static class Product
            {
                public static string List()
                {
                    return $"api/v1/product";
                }
                public static string Add()
                {
                    return $"api/v1/product";
                }

                public static string Delete(int productId)
                {
                    return $"api/v1/product/{productId}";
                }
                public static string Get(int productId)
                {
                    return $"api/v1/product/{productId}";
                }
                public static string List(int pageIndex, int pageCount)
                {
                    return $"api/v1/product?pageIndex={pageIndex}&pageCount={pageCount}";
                }
            }
            public static class Flags
            {
                public static string Rollout(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flag/{featureId}/rollout";
                }
                public static string Delete(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flag/{featureId}";
                }
                public static string Get(int productId, int featureId)
                {
                    return $"api/v1/product/{productId}/flag/{featureId}";
                }
                public static string List(int productId)
                {
                    return $"api/v1/product/{productId}/flag";
                }
                public static string List(int productId, int pageIndex, int pageCount)
                {
                    return $"api/v1/product/{productId}/flag?pageIndex={pageIndex}&pageCount={pageCount}";
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
                    return $"api/v1/toggle/{toggleId}/parameter/reveal";
                }
                public static string KnownTypes()
                {
                    return $"api/v1/toggle/knowntype";
                }
                public static string Post()
                {
                    return "api/v1/toggle";
                }
                public static string PostParameter()
                {
                    return "api/v1/toggle/parameter";
                }
            }

            public static class Tags
            {
                public static string Delete(string tag, int featureId)
                {
                    return $"api/v1/tags/{tag}/flags/{featureId}";
                }
            }
        }
    }
}
