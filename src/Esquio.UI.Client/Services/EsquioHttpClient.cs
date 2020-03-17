using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Features.Add;
using Esquio.UI.Api.Shared.Models.Features.List;
using Esquio.UI.Api.Shared.Models.Features.Update;
using Esquio.UI.Api.Shared.Models.Products.Add;
using Esquio.UI.Api.Shared.Models.Products.AddRing;
using Esquio.UI.Api.Shared.Models.Products.Details;
using Esquio.UI.Api.Shared.Models.Products.List;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Client.Services
{
    public interface IEsquioHttpClient
    {
        Task<PaginatedResult<ListProductResponseDetail>> GetProductsList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default);

        Task<DetailsProductResponse> GetProduct(string productName, CancellationToken cancellationToken = default);

        Task<bool> AddProduct(AddProductRequest addProductRequest, CancellationToken cancellationToken = default);

        Task<bool> DeleteProduct(string productName, CancellationToken cancellationToken = default);

        Task<bool> AddProductRing(string productName, AddRingRequest addRingRequest, CancellationToken cancellationToken = default);

        Task<bool> DeleteProductRing(string productName, string ringName, CancellationToken cancellationToken = default);

        Task<PaginatedResult<ListFeatureResponseDetail>> GetProductFeaturesList(string productName, int? pageIndex, int? pageCount, CancellationToken cancellationToken = default);

        Task<bool> ToggleFeature(string productName, string featureName, UpdateFeatureRequest updateFeatureRequest, CancellationToken cancellationToken = default);

        Task<bool> RolloutFeature(string productName, string featureName, CancellationToken cancellationToken = default);

        Task<bool> RollbackFeature(string productName, string featureName, CancellationToken cancellationToken = default);

        Task<bool> ArchiveFeature(string productName, string featureName, CancellationToken cancellationToken = default);

        Task<bool> AddFeature(string productName, AddFeatureRequest addFeatureRequest, CancellationToken cancellationToken = default);

        Task<bool> DeleteFeature(string productName, string featureName, CancellationToken cancellationToken = default);
    }

    public class EsquioHttpClient
        : IEsquioHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public EsquioHttpClient(HttpClient httpClient, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _accessTokenProvider = accessTokenProvider ?? throw new ArgumentNullException(nameof(accessTokenProvider));
        }

        public async Task<PaginatedResult<ListProductResponseDetail>> GetProductsList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("api/products?");

            if (pageIndex != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("pageIndex") + "=").Append(System.Uri.EscapeDataString(ConvertToString(pageIndex, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (pageCount != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("pageCount") + "=").Append(System.Uri.EscapeDataString(ConvertToString(pageCount, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }

            urlBuilder.Length--;

            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<PaginatedResult<ListProductResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                    catch (JsonException exception)
                    {
                        //TODO: que hacemos aqui
                    }
                }

                //TODO: que hacemos aqui?
                return null;
            }
        }

        public async Task<DetailsProductResponse> GetProduct(string productName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri($"api/products/{productName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<DetailsProductResponse>(content, new JsonSerializerSettings());
                    }
                    catch (JsonException exception)
                    {
                        //TODO: que hacemos aqui
                    }
                }

                //TODO: que hacemos aqui?
                return null;
            }
        }

        public async Task<bool> AddProduct(AddProductRequest addProductRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("api/products", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var content = JsonConvert.SerializeObject(addProductRequest);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json"); ;

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> DeleteProduct(string productName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/products/{productName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> AddProductRing(string productName, AddRingRequest addRingRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri($"api/products/{productName}/ring", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var content = JsonConvert.SerializeObject(addRingRequest);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json"); ;

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> DeleteProductRing(string productName, string ringName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/products/{productName}/ring/{ringName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<PaginatedResult<ListFeatureResponseDetail>> GetProductFeaturesList(string productName, int? pageIndex, int? pageCount, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"api/products/{productName}/features?");

            if (pageIndex != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("pageIndex") + "=").Append(System.Uri.EscapeDataString(ConvertToString(pageIndex, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (pageCount != null)
            {
                urlBuilder.Append(Uri.EscapeDataString("pageCount") + "=").Append(System.Uri.EscapeDataString(ConvertToString(pageCount, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }

            urlBuilder.Length--;

            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<PaginatedResult<ListFeatureResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                    catch (JsonException exception)
                    {
                        //TODO: que hacemos aqui
                    }
                }
                //TODO: que hacemos aqui?
                return null;
            }
        }
        public async Task<bool> ToggleFeature(string productName, string featureName, UpdateFeatureRequest updateFeatureRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var content = JsonConvert.SerializeObject(updateFeatureRequest);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json"); ;

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public async Task<bool> RolloutFeature(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/rollout", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public async Task<bool> RollbackFeature(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/rollback", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public async Task<bool> ArchiveFeature(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/archive", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public async Task<bool> AddFeature(string productName, AddFeatureRequest addFeatureRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri($"api/products/{productName}/features", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var content = JsonConvert.SerializeObject(addFeatureRequest);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json"); ;

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public async Task<bool> DeleteFeature(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        private async Task<HttpRequestMessage> CreateHttpRequestMessageAsync()
        {
            var msg = new HttpRequestMessage();
            var token = await GetTokenAsync();

            if (token != null)
            {
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            return msg;
        }

        private async Task<AccessToken> GetTokenAsync()
        {
            const string AUTHORIZATION_HEADER_NAME = "Authorization";

            if (!_httpClient.DefaultRequestHeaders.TryGetValues(AUTHORIZATION_HEADER_NAME, out _))
            {
                var tokenResult = await _accessTokenProvider.RequestAccessToken();

                if (tokenResult.Status == AccessTokenResultStatus.Success)
                {
                    if (tokenResult.TryGetToken(out AccessToken token))
                    {
                        return token;
                    }
                }
            }

            return null;
        }

        string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is System.Enum)
            {
                string name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }
                }
            }
            else if (value is bool)
            {
                return Convert.ToString(value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return Convert.ToBase64String((byte[])value);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            return Convert.ToString(value, cultureInfo);
        }
    }
}
