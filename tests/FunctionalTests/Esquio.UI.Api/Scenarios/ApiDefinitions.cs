namespace FunctionalTests.Esquio.UI.Api.Scenarios
{
    public static class ApiDefinitions
    {
        public static class V2
        {
            public static class Product
            {
                public static string List()
                {
                    return $"api/products?api-version=2.0";
                }
                public static string Add()
                {
                    return $"api/products?api-version=2.0";
                }

                public static string Update(string productName)
                {
                    return $"api/products/{productName}?api-version=2.0";
                }

                public static string Delete(string name)
                {
                    return $"api/products/{name}?api-version=2.0";
                }
                public static string Get(string name)
                {
                    return $"api/products/{name}?api-version=2.0";
                }
                public static string List(int pageIndex = 0, int pageCount = 10)
                {
                    return $"api/products?pageIndex={pageIndex}&pageCount={pageCount}&api-version=2.0";
                }
            }

            public static class Features
            {
                public static string Add(string productName)
                {
                    return $"api/products/{productName}/features?api-version=2.0";
                }
                public static string Update(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}?api-version=2.0";
                }
                public static string Rollout(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/rollout?api-version=2.0";
                }
                public static string Rollback(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/rollback?api-version=2.0";
                }
                public static string Delete(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}?api-version=2.0";
                }
                public static string Get(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}?api-version=2.0";
                }
                public static string List(string productName)
                {
                    return $"api/products/{productName}/features";
                }
                public static string List(string productName, int pageIndex, int pageCount)
                {
                    return $"api/products/{productName}/features?pageIndex={pageIndex}&pageCount={pageCount}&api-version=2.0";
                }
            }

            public static class ApiKeys
            {
                public static string Add()
                {
                    return $"api/apikeys?api-version=2.0";
                }
                public static string Delete(string name)
                {
                    return $"api/apikeys/{name}?api-version=2.0";
                }
                public static string Get(string name)
                {
                    return $"api/apikeys/{name}?api-version=2.0";
                }
                public static string List()
                {
                    return $"api/apikeys?api-version=2.0";
                }
                public static string List(int pageIndex, int pageCount)
                {
                    return $"api/apikeys?pageIndex={pageIndex}&pageCount={pageCount}&api-version=2.0";
                }
            }

            public static class Toggles
            {
                public static string Get(string productName, string featureName, string toggleType)
                {
                    return $"api/products/{productName}/features/{featureName}/toggles/{toggleType}?api-version=2.0";
                }
                public static string Delete(string productName, string featureName, string toggleType)
                {
                    return $"api/products/{productName}/features/{featureName}/toggles/{toggleType}?api-version=2.0";
                }
                public static string Reveal(string toggleType)
                {
                    return $"api/toggles/parameters/{toggleType}?api-version=2.0";
                }
                public static string KnownTypes()
                {
                    return $"api/toggles/types?api-version=2.0";
                }
                public static string Post()
                {
                    return "api/toggles?api-version=2.0";
                }
                public static string PostParameter()
                {
                    return $"api/toggles/parameters?api-version=2.0";
                }
            }

            public static class Tags
            {
                public static string List(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/tags?api-version=2.0";
                }

                public static string Untag(string productName,string featureName,string tag)
                {
                    return $"api/products/{productName}/features/{featureName}/tags/untag/{tag}?api-version=2.0";
                }

                public static string Tag(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/tags/tag?api-version=2.0";
                }
            }

            public static class Users
            {
                public static string My()
                {
                    return $"api/users/my?api-version=2.0";
                }

                public static string List(int pageIndex = 0, int pageCount = 1)
                {
                    return $"api/users?pageIndex={pageIndex}&pageCount={pageCount}&api-version=2.0";
                }

                public static string Add()
                {
                    return $"api/users?api-version=2.0";
                }

                public static string Delete(string subjectId)
                {
                    return $"api/users/{subjectId}?api-version=2.0";
                }

                public static string Details(string subjectId)
                {
                    return $"api/users/{subjectId}?api-version=2.0";
                }
            }
        }
    }
}
