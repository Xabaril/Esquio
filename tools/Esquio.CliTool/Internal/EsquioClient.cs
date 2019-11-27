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

        public async Task<HttpResponseMessage> ListProductsAsync(int pageIndex = 0, int pageCount = 10)
        {
            return await _httpClient.GetAsync($"api/products?pageIndex={pageIndex}&pageCount={pageCount}");
        }

        public async Task<HttpResponseMessage> AddProductAsync(string productName, string description)
        {
            var jsonContent = JsonSerializer.Serialize(new
            {
                Name = productName,
                Description = description,
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await _httpClient.PostAsync("api/products", content);
        }

        public async Task<HttpResponseMessage> RemoveProductAsync(string productName)
        {
            return await _httpClient.DeleteAsync($"api/products/{productName}");
        }

        public async Task<HttpResponseMessage> ListFeaturesAsync(string productName, int pageIndex = 0, int pageCount = 10)
        {
            return await _httpClient.GetAsync($"api/products/{productName}/features?pageIndex={pageIndex}&pageCount={pageCount}");
        }

        public async Task<HttpResponseMessage> RolloutFeatureAsync(string productName, string featureName)
        {
            return await _httpClient.PutAsync($"api/products/{productName}/features/{featureName}/rollout", new StringContent(string.Empty));
        }

        public async Task<HttpResponseMessage> RollbackFeatureAsync(string productName, string featureName)
        {
            return await _httpClient.PutAsync($"api/products/{productName}/features/{featureName}/rollback", new StringContent(string.Empty));
        }

        public async Task<HttpResponseMessage> ListTogglesAsync(string productName, string featureName)
        {
            return await _httpClient.GetAsync($"api/products/{productName}/features/{featureName}");
        }

        public async Task<HttpResponseMessage> GetToggleAsync(string productName, string featureName, string toggleType)
        {
            return await _httpClient.GetAsync($"api/products/{productName}/features/{featureName}/toggles/{toggleType}");
        }

        public async Task<HttpResponseMessage> SetParameterValue(string productName, string featureName, string toggleType, string name, string value)
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

            return await _httpClient.PostAsync($"api/toggles/parameters", content);
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

            httpClient.DefaultRequestHeaders
                .Add("X-Api-Key", apikey);

            httpClient.DefaultRequestHeaders
                .Add("X-Api-Version", HTTP_API_VERSION);

            return new EsquioClient(httpClient);
        }
    }
}
