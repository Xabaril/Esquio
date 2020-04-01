using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.ApiKeys.Add;
using Esquio.UI.Api.Shared.Models.ApiKeys.List;
using Esquio.UI.Api.Shared.Models.Audit.List;
using Esquio.UI.Api.Shared.Models.Features.Add;
using Esquio.UI.Api.Shared.Models.Features.Details;
using Esquio.UI.Api.Shared.Models.Features.List;
using Esquio.UI.Api.Shared.Models.Features.Update;
using Esquio.UI.Api.Shared.Models.Permissions.Add;
using Esquio.UI.Api.Shared.Models.Permissions.Details;
using Esquio.UI.Api.Shared.Models.Permissions.List;
using Esquio.UI.Api.Shared.Models.Permissions.My;
using Esquio.UI.Api.Shared.Models.Permissions.Update;
using Esquio.UI.Api.Shared.Models.Products.Add;
using Esquio.UI.Api.Shared.Models.Products.AddRing;
using Esquio.UI.Api.Shared.Models.Products.Details;
using Esquio.UI.Api.Shared.Models.Products.List;
using Esquio.UI.Api.Shared.Models.Tags.Add;
using Esquio.UI.Api.Shared.Models.Tags.List;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
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

        Task<MyResponse> GetMy(CancellationToken cancellationToken = default);
        Task<PaginatedResult<ListUsersResponseDetail>> GetUserList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default);
        Task<DetailsPermissionResponse> GetUserDetails(string subjectId, CancellationToken cancellationToken = default);
        Task<bool> AddUserPermission(AddPermissionRequest addPermissionRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserPermission(string subjectId, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserPermission(UpdatePermissionRequest updatePermissionRequest, CancellationToken cancellationToken = default);

        Task<PaginatedResult<ListApiKeyResponseDetail>> GetApiKeyList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default);
        Task<AddApiKeyResponse> AddNewApiKey(AddApiKeyRequest addApiKeyRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteApiKey(string apiKeyName, CancellationToken cancellationToken = default);

        Task<DetailsFeatureResponse> GetFeatureDetails(string productName, string featureName, CancellationToken cancellationToken = default);

        Task<bool> DeleteToggle(string productName, string featureName, string toggleType, CancellationToken cancellationToken = default);

        Task<IEnumerable<TagResponseDetail>> GetTagsList(string productName, string featureName, CancellationToken cancellationToken = default);
        Task<bool> AddTag(string productName, string featureName, AddTagRequest addTagRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteTag(string productName, string featureName, string tag, CancellationToken cancellationToken = default);

        Task<PaginatedResult<ListAuditResponseDetail>> GetAuditList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default);

    }

    public class EsquioHttpClient
        : IEsquioHttpClient
    {
        const string API_VERSION_HEADER_NAME = "x-api-version";
        const string API_VERSION_HEADER_VALUE = "3.0";
        const string DEFAULT_CONTENT_TYPE = "application/json";

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
            urlBuilder.Append($"api/products?");

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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<PaginatedResult<ListProductResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "An exception is throwed when trying to get the products list from server.!");
                }

                return null;
            }
        }

        public async Task<DetailsProductResponse> GetProduct(string productName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri($"api/products/{productName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<DetailsProductResponse>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get the product {productName} from the server.!");
                }

                return null;
            }
        }

        public async Task<bool> AddProduct(AddProductRequest addProductRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri($"api/products", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                var content = JsonConvert.SerializeObject(addProductRequest);
                request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to add product {addProductRequest}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete product {productName}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var content = JsonConvert.SerializeObject(addRingRequest);
                    request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE);

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to add {addRingRequest} to product {productName}. !");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete ring {ringName} on product {productName}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<PaginatedResult<ListFeatureResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get feature list on product {productName}.!");
                }

                return null;
            }
        }
        public async Task<bool> ToggleFeature(string productName, string featureName, UpdateFeatureRequest updateFeatureRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var content = JsonConvert.SerializeObject(updateFeatureRequest);
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json"); ;

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to toggle feature {featureName} on product {productName} with data {updateFeatureRequest}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to rolled out feature {featureName} on product {productName}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to rolled back feature {featureName} on product {productName}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to archive feature {featureName} on product {productName}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var content = JsonConvert.SerializeObject(addFeatureRequest);
                    request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE); ;

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to add new  feature {addFeatureRequest} on product {productName}.!");
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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete feature {featureName} on product {productName}.!");
                }
                return false;
            }
        }

        public async Task<MyResponse> GetMy(CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri("api/permissions/my", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<MyResponse>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get the logged user.!");
                }

                return null;
            }
        }

        public async Task<PaginatedResult<ListUsersResponseDetail>> GetUserList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"api/permissions?");

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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<PaginatedResult<ListUsersResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get users permission list.!");
                }

                return null;
            }
        }

        public async Task<bool> AddUserPermission(AddPermissionRequest addPermissionRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri($"api/permissions", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var content = JsonConvert.SerializeObject(addPermissionRequest);
                    request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE); ;

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to add new permission for subject {addPermissionRequest.SubjectId} acting as {addPermissionRequest.ActAs}.!");
                }
                
                return false;
            }
        }

        public async Task<bool> UpdateUserPermission(UpdatePermissionRequest updatePermissionRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Put;
                request.RequestUri = new Uri($"api/permissions", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var content = JsonConvert.SerializeObject(updatePermissionRequest);
                    request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE); ;

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to update permission for subject {updatePermissionRequest.SubjectId} acting as {updatePermissionRequest.ActAs}.!");
                }

                return false;
            }
        }

        public async Task<bool> DeleteUserPermission(string subjectId, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/permissions/{subjectId}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete permission for subject {subjectId}.!");
                }

                return false;
            }
        }

        public async Task<DetailsPermissionResponse> GetUserDetails(string subjectId, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri($"api/permissions/{subjectId}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<DetailsPermissionResponse>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get the logged user.!");
                }

                return null;
            }
        }

        public async Task<PaginatedResult<ListApiKeyResponseDetail>> GetApiKeyList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"api/apikeys?");

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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<PaginatedResult<ListApiKeyResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get api keys.!");
                }

                return null;
            }
        }

        public async Task<AddApiKeyResponse> AddNewApiKey(AddApiKeyRequest addApiKeyRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri($"api/apikeys", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var content = JsonConvert.SerializeObject(addApiKeyRequest);
                    request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE); ;

                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {

                        var responseContent = await response.Content
                            .ReadAsStringAsync();
                        
                        return JsonConvert.DeserializeObject<AddApiKeyResponse>(responseContent);
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to add new api key {addApiKeyRequest.Name}.!");
                }

                return null;
            }
        }

        public async Task<bool> DeleteApiKey(string apiKeyName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/apikeys/{apiKeyName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete api key {apiKeyName}.!");
                }

                return false;
            }
        }

        public async Task<DetailsFeatureResponse> GetFeatureDetails(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<DetailsFeatureResponse>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get the feature {featureName} on product {productName} from the server.!");
                }

                return null;
            }
        }

        public async Task<bool> DeleteToggle(string productName, string featureName, string toggleType, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/toggles/{toggleType}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete toggle {toggleType} on feature {featureName} on product {productName}.!");
                }
                return false;
            }
        }

        public async Task<IEnumerable<TagResponseDetail>> GetTagsList(string productName, string featureName, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/tags", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<IEnumerable<TagResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get the tags list on feature {featureName} on product {productName} from server.!");
                }

                return null;
            }
        }

        public async Task<bool> AddTag(string productName, string featureName, AddTagRequest addTagRequest, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/tags/tag", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                var content = JsonConvert.SerializeObject(addTagRequest);
                request.Content = new StringContent(content, Encoding.UTF8, DEFAULT_CONTENT_TYPE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to add tag {addTagRequest}.!");
                }

                return false;
            }
        }

        public async Task<bool> DeleteTag(string productName, string featureName, string tag, CancellationToken cancellationToken = default)
        {
            using (var request = await CreateHttpRequestMessageAsync())
            {
                request.Method = HttpMethod.Delete;
                request.RequestUri = new Uri($"api/products/{productName}/features/{featureName}/tags/untag/{tag}", UriKind.RelativeOrAbsolute);
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to delete tag {productName}.!");
                }

                return false;
            }
        }

        public async Task<PaginatedResult<ListAuditResponseDetail>> GetAuditList(int? pageIndex, int? pageCount, CancellationToken cancellationToken = default)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append($"api/audit?");

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
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(DEFAULT_CONTENT_TYPE));
                request.Headers.Add(API_VERSION_HEADER_NAME, API_VERSION_HEADER_VALUE);

                try
                {
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<PaginatedResult<ListAuditResponseDetail>>(
                            content, new JsonSerializerSettings());
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, $"An exception is throwed when trying to get audit information!.");
                }

                return null;
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
                    Log.Debug("Token result is valid and we can reuse it.");

                    if (tokenResult.TryGetToken(out AccessToken token))
                    {
                        Log.Debug($"Recovered token is {token.Value}");

                        return token;
                    }
                }
                else
                {
                    Log.Warning("Token is expired, need to get a new one");
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
