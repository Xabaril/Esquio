using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Toggles
{
    public interface ILocationProviderService
    {
        Task<string> GetCountryName(string ipaddress, CancellationToken cancellationToken = default);
    }

    public class IPApiLocationProviderService
        : ILocationProviderService
    {
        const string IP_API_LOCATION_SERVICE = nameof(IP_API_LOCATION_SERVICE);
        const string BASE_ADDRESS = "http://ip-api.com/json/";

        static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IHttpClientFactory _httpClientFactory;

        public IPApiLocationProviderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
       
        public async Task<string> GetCountryName(string ipaddress, CancellationToken cancellationToken = default)
        {
            var data = await GetIpDataFrom(ipaddress, cancellationToken);

            return data?.Country;
        }
              
        private async Task<IPApiData> GetIpDataFrom(string ipaddress, CancellationToken cancellationToken = default)
        {
#if DEBUG
            if(ipaddress == "0.0.0.1")
            {
                ipaddress = "213.97.0.42";
            }
#endif
            var httpClient = _httpClientFactory
                .CreateClient(IP_API_LOCATION_SERVICE);

            var response = await httpClient.GetAsync($"{BASE_ADDRESS}{ipaddress}");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.ReadAsync<IPApiData>(
                    utf8Json: stream,
                    options: _serializerOptions,
                    cancellationToken);
            }

            return default;
        }

        public class IPApiData
        {
            public string Query { get; set; }
            public string Status { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string Region { get; set; }
            public string RegionName { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public float Lat { get; set; }
            public float Lon { get; set; }
            public string Timezone { get; set; }
            public string Isp { get; set; }
            public string Org { get; set; }
            public string _as { get; set; }
        }
    }
}
