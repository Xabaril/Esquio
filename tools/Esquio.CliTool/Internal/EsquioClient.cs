using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Esquio.CliTool.Internal
{
    public class EsquioClient
    {
        private readonly HttpClient _httpClient;

        private EsquioClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> ListProductsAsync()
        {
            return await _httpClient.GetAsync($"api/v1/products?pageIndex=0&pageCount=99");
        }

        public async Task<HttpResponseMessage> AddProductAsync(string productName, string description)
        {
            var jsonContent = JsonSerializer.Serialize(new
            {
                Name = productName,
                Description = description,
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return await _httpClient.PostAsync("api/v1/products", content);
        }

        public async Task<HttpResponseMessage> RemoveProductAsync(int productId)
        {
            return await _httpClient.DeleteAsync($"api/v1/products/{productId}");
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
