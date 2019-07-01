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
                    return $"api/v1/products";
                }
                public static string Add()
                {
                    return $"api/v1/products";
                }

                public static string Update()
                {
                    return $"api/v1/products";
                }

                public static string Delete(int productId)
                {
                    return $"api/v1/products/{productId}";
                }
                public static string Get(int productId)
                {
                    return $"api/v1/products/{productId}";
                }
                public static string List(int pageIndex, int pageCount)
                {
                    return $"api/v1/products?pageIndex={pageIndex}&pageCount={pageCount}";
                }
            }

            public static class Flags
            {
                public static string Add()
                {
                    return $"api/v1/flags";
                }
                public static string Rollout(int featureId)
                {
                    return $"api/v1/flags/{featureId}/rollout";
                }
                public static string Rollback(int featureId)
                {
                    return $"api/v1/flags/{featureId}/rollback";
                }
                public static string Delete(int featureId)
                {
                    return $"api/v1/flags/{featureId}";
                }
                public static string Get(int featureId)
                {
                    return $"api/v1/flags/{featureId}";
                }
                public static string List(int productId)
                {
                    return $"api/v1/products/{productId}/flags";
                }
                public static string List(int productId, int pageIndex, int pageCount)
                {
                    return $"api/v1/products/{productId}/flags?pageIndex={pageIndex}&pageCount={pageCount}";
                }
            }

            public static class ApiKeys
            {
                public static string Add()
                {
                    return $"api/v1/apikeys/";
                }
                public static string Delete(int apiKeyId)
                {
                    return $"api/v1/apikeys/{apiKeyId}";
                }
                public static string Get(int apiKeyId)
                {
                    return $"api/v1/apikeys/{apiKeyId}";
                }
                public static string List()
                {
                    return $"api/v1/apikeys/";
                }
                public static string List(int pageIndex, int pageCount)
                {
                    return $"api/v1/apikeys?pageIndex={pageIndex}&pageCount={pageCount}";
                }
            }

            public static class Toggles
            {
                public static string Get(int toggleId)
                {
                    return $"api/v1/toggles/{toggleId}";
                }
                public static string Delete(int toggleId)
                {
                    return $"api/v1/toggles/{toggleId}";
                }
                public static string Reveal(string toggleType)
                {
                    return $"api/v1/toggles/parameters/{toggleType}";
                }
                public static string KnownTypes()
                {
                    return $"api/v1/toggles/types";
                }
                public static string Post()
                {
                    return "api/v1/toggles";
                }
                public static string PostParameter(int toggleId)
                {
                    return $"api/v1/toggles/{toggleId}/parameters";
                }
            }

            public static class Tags
            {
                public static string List(int featureId)
                {
                    return $"api/v1/tags/{featureId}";
                }

                public static string Untag(string tag, int featureId)
                {
                    return $"api/v1/tags/{featureId}/{tag}";
                }

                public static string Tag(int featureId)
                {
                    return $"api/v1/tags/{featureId}";
                }
            }
        }
    }
}
