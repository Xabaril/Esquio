namespace FunctionalTests.Esquio.UI.Api.Scenarios
{
    public static class ApiDefinitions
    {
        public static class V3
        {
            public static class Product
            {
                public static string List()
                {
                    return $"api/products?api-version=3.0";
                }
                public static string Add()
                {
                    return $"api/products?api-version=3.0";
                }
                public static string AddRing(string productName)
                {
                    return $"api/products/{productName}/ring?api-version=3.0";
                }

                public static string Update(string productName)
                {
                    return $"api/products/{productName}?api-version=3.0";
                }

                public static string Delete(string name)
                {
                    return $"api/products/{name}?api-version=3.0";
                }
                public static string DeleteRing(string productName, string ringName)
                {
                    return $"api/products/{productName}/ring/{ringName}?api-version=3.0";
                }
                public static string Get(string name)
                {
                    return $"api/products/{name}?api-version=3.0";
                }
                public static string List(int pageIndex = 0, int pageCount = 10)
                {
                    return $"api/products?pageIndex={pageIndex}&pageCount={pageCount}&api-version=3.0";
                }
            }
            public static class Audit
            {
                public static string List()
                {
                    return $"api/audit?api-version=3.0";
                }
                public static string List(int pageIndex, int pageCount)
                {
                    return $"api/audit?pageIndex={pageIndex}&pageCount={pageCount}&api-version=3.0";
                }
            }

            public static class Features
            {
                public static string Add(string productName)
                {
                    return $"api/products/{productName}/features?api-version=3.0";
                }
                public static string Update(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}?api-version=3.0";
                }
                public static string Rollout(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/rollout?api-version=3.0";
                }
                public static string Rollback(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/rollback?api-version=3.0";
                }

                public static string Archive(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/archive?api-version=3.0";
                }
                public static string Delete(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}?api-version=3.0";
                }
                public static string Get(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}?api-version=3.0";
                }
                public static string List(string productName)
                {
                    return $"api/products/{productName}/features";
                }
                public static string List(string productName, int pageIndex, int pageCount)
                {
                    return $"api/products/{productName}/features?pageIndex={pageIndex}&pageCount={pageCount}&api-version=3.0";
                }
            }

            public static class Store
            {
                public static string Get(string productName, string featureName, string ringName)
                {
                    return $"api/store/product/{productName}/feature/{featureName}?ring={ringName}&api-version=3.0";
                }
            }

            public static class ApiKeys
            {
                public static string Add()
                {
                    return $"api/apikeys?api-version=3.0";
                }
                public static string Delete(string name)
                {
                    return $"api/apikeys/{name}?api-version=3.0";
                }
                public static string Get(string name)
                {
                    return $"api/apikeys/{name}?api-version=3.0";
                }
                public static string List()
                {
                    return $"api/apikeys?api-version=3.0";
                }
                public static string List(int pageIndex, int pageCount)
                {
                    return $"api/apikeys?pageIndex={pageIndex}&pageCount={pageCount}&api-version=3.0";
                }
            }

            public static class Toggles
            {
                public static string Get(string productName, string featureName, string toggleType)
                {
                    return $"api/products/{productName}/features/{featureName}/toggles/{toggleType}?api-version=3.0";
                }
                public static string Delete(string productName, string featureName, string toggleType)
                {
                    return $"api/products/{productName}/features/{featureName}/toggles/{toggleType}?api-version=3.0";
                }
                public static string Reveal(string toggleType)
                {
                    return $"api/toggles/parameters/{toggleType}?api-version=3.0";
                }
                public static string KnownTypes()
                {
                    return $"api/toggles/types?api-version=3.0";
                }
                public static string Post()
                {
                    return "api/toggles?api-version=3.0";
                }
                public static string PostParameter()
                {
                    return $"api/toggles/parameters?api-version=3.0";
                }
            }

            public static class Tags
            {
                public static string List(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/tags?api-version=3.0";
                }

                public static string Untag(string productName, string featureName, string tag)
                {
                    return $"api/products/{productName}/features/{featureName}/tags/untag/{tag}?api-version=3.0";
                }

                public static string Tag(string productName, string featureName)
                {
                    return $"api/products/{productName}/features/{featureName}/tags/tag?api-version=3.0";
                }
            }

            public static class Permissions
            {
                public static string My()
                {
                    return $"api/permissions/my?api-version=3.0";
                }

                public static string List(int pageIndex = 0, int pageCount = 1)
                {
                    return $"api/permissions?pageIndex={pageIndex}&pageCount={pageCount}&api-version=3.0";
                }

                public static string Add()
                {
                    return $"api/permissions?api-version=3.0";
                }

                public static string Delete(string subjectId)
                {
                    return $"api/permissions/{subjectId}?api-version=3.0";
                }

                public static string Details(string subjectId)
                {
                    return $"api/permissions/{subjectId}?api-version=3.0";
                }
            }
        }
    }
}
