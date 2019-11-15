using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Esquio.CliTool.Internal
{
    public class EsquioClient : IDisposable
    {
        const string HTTP_API_VERSION = "2.0";

        private readonly HttpClient _httpClient;

        private EsquioClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> ListProductsAsync()
        {
            return await _httpClient.GetAsync($"api/products?pageIndex=0&pageCount=99?api-version={HTTP_API_VERSION}");
        }

        public async Task<HttpResponseMessage> AddProductAsync(string productName, string description)
        {
            var jsonContent = JsonSerializer.Serialize(new
            {
                Name = productName,
                Description = description,
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await _httpClient.PostAsync("api/products?api-version={HTTP_API_VERSION}", content);
        }

        public async Task<HttpResponseMessage> RemoveProductAsync(string productName)
        {
            return await _httpClient.DeleteAsync($"api/products/{productName}?api-version={HTTP_API_VERSION}");
        }

        public async Task<HttpResponseMessage> ListFeaturesAsync(string productName)
        {
            return await _httpClient.GetAsync($"api/products/{productName}/features?api-version={HTTP_API_VERSION}");
        }

        public async Task<HttpResponseMessage> RolloutFeatureAsync(string productName, string featureName)
        {
            return await _httpClient.PutAsync($"api/products/{productName}/features/{featureName}/rollout?api-version={HTTP_API_VERSION}", new StringContent(string.Empty));
        }

        public async Task<HttpResponseMessage> RollbackFeatureAsync(string productName, string featureName)
        {
            return await _httpClient.PutAsync($"api/products/{productName}/features/{featureName}/rollback?api-version={HTTP_API_VERSION}", new StringContent(string.Empty));
        }

        public async Task<HttpResponseMessage> ListTogglesAsync(string productName, string featureName)
        {
            return await _httpClient.GetAsync($"api/products/{productName}/features/{featureName}?api-version={HTTP_API_VERSION}");
        }

        public async Task<HttpResponseMessage> GetToggleAsync(string productName, string featureName, string toggleType)
        {
            return await _httpClient.GetAsync($"api/products/{productName}/features/{featureName}/toggles/{toggleType}?api-version={HTTP_API_VERSION}");
        }

        public async Task<HttpResponseMessage> SetParameterValue(string productName, string featureName,string toggleType, string name, string value)
        {
            var jsonContent = JsonSerializer.Serialize(new
            {
                ProductName = productName,
                FeatureName = featureName,
                ToggleType = toggleType,
                Name = name,
                Value = value,
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await _httpClient.PostAsync($"api/toggles/parameters?api-version={HTTP_API_VERSION}", content);
        }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        public static EsquioClient Create(string uri, string apikey)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(uri)
            };

            httpClient.DefaultRequestHeaders.Add("x-api-key", apikey);

            return new EsquioClient(httpClient);
        }
    }
}
