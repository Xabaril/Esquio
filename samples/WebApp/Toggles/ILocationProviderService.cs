using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Toggles
{
    public interface ILocationProviderService
    {
        Task<string> GetCountryName(string ipaddress);

        Task<string> GetCountryCode(string ipaddress);

        Task<string> GetRegionName(string ipaddress);
    }

    public class IPApiLocationProviderService
        : ILocationProviderService
    {
        const string IP_API_LOCATION_SERVICE = nameof(IP_API_LOCATION_SERVICE);
        const string BASE_ADDRESS = "http://ip-api.com/json/";

        private readonly IHttpClientFactory _httpClientFactory;

        public IPApiLocationProviderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<string> GetCountryCode(string ipaddress)
        {
            var data = await GetIpDataFrom(ipaddress);

            return data?.CountryCode;
        }

        public async Task<string> GetCountryName(string ipaddress)
        {
            var data = await GetIpDataFrom(ipaddress);

            return data?.Country;
        }

        public async Task<string> GetRegionCode(string ipaddress)
        {
            var data = await GetIpDataFrom(ipaddress);

            return data?.Region;
        }

        public async Task<string> GetRegionName(string ipaddress)
        {
            var data = await GetIpDataFrom(ipaddress);

            return data?.RegionName;
        }

        private async Task<IPApiData> GetIpDataFrom(string ipaddress)
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
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IPApiData>(content);
            }

            return default;
        }

        public class IPApiData
        {
            [JsonProperty("query")]
            public string Query { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("country")]
            public string Country { get; set; }
            [JsonProperty("countryCode")]
            public string CountryCode { get; set; }
            [JsonProperty("region")]
            public string Region { get; set; }
            [JsonProperty("regionName")]
            public string RegionName { get; set; }
            [JsonProperty("city")]
            public string City { get; set; }
            [JsonProperty("zip")]
            public string Zip { get; set; }
            [JsonProperty("lat")]
            public float Lat { get; set; }
            [JsonProperty("lon")]
            public float Lon { get; set; }
            [JsonProperty("timezone")]
            public string Timezone { get; set; }
            [JsonProperty("isp")]
            public string Isp { get; set; }
            [JsonProperty("org")]
            public string Org { get; set; }
            [JsonProperty("_as")]
            public string _as { get; set; }
        }
    }
}
