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

                public static string Update(string productName)
                {
                    return $"api/v1/products/{productName}";
                }

                public static string Delete(string name)
                {
                    return $"api/v1/products/{name}";
                }
                public static string Get(string name)
                {
                    return $"api/v1/products/{name}";
                }
                public static string List(int pageIndex = 0, int pageCount = 10)
                {
                    return $"api/v1/products?pageIndex={pageIndex}&pageCount={pageCount}";
                }
            }

            public static class Features
            {
                public static string Add(string productName)
                {
                    return $"api/v1/products/{productName}/features";
                }
                public static string Update(string productName, string featureName)
                {
                    return $"api/v1/products/{productName}/features/{featureName}";
                }
                public static string Rollout(string productName, string featureName)
                {
                    return $"api/v1/products/{productName}/features/{featureName}/rollout";
                }
                public static string Rollback(string productName, string featureName)
                {
                    return $"api/v1/products/{productName}/features/{featureName}/rollback";
                }
                public static string Delete(string productName, string featureName)
                {
                    return $"api/v1/products/{productName}/features/{featureName}";
                }
                public static string Get(string productName, string featureName)
                {
                    return $"api/v1/products/{productName}/features/{featureName}";
                }
                public static string List(string productName)
                {
                    return $"api/v1/products/{productName}/features";
                }
                public static string List(string productName, int pageIndex, int pageCount)
                {
                    return $"api/v1/products/{productName}/features?pageIndex={pageIndex}&pageCount={pageCount}";
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
                public static string Delete(string productName, string featureName, string toggleType)
                {
                    return $"api/v1/products/{productName}/features/{featureName}/toggles/{toggleType}";
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
                public static string PostParameter()
                {
                    return $"api/v1/toggles/parameters";
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

            public static class Users
            {
                public static string My()
                {
                    return $"api/v1/users/my";
                }

                public static string List(int pageIndex = 0, int pageCount = 1)
                {
                    return $"api/v1/users?pageIndex={pageIndex}&pageCount={pageCount}";
                }

                public static string Add()
                {
                    return $"api/v1/users";
                }

                public static string Delete(string subjectId)
                {
                    return $"api/v1/users/{subjectId}";
                }

                public static string Details(string subjectId)
                {
                    return $"api/v1/users/{subjectId}";
                }
            }
        }
    }
}
