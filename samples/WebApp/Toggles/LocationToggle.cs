using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Toggles
{
    public class LocationToggle
        : IToggle
    {
        const string BASE_ADDRESS = "http://ip-api.com/json/";

        static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true
        };

        static char[] split_characters = new char[] { ';' };

        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationToggle(IRuntimeFeatureStore featureStore, IHttpContextAccessor httpContextAccessor)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException();
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            var allowedCountries = (string)data.Countries;
            var location = await GetLocationFromIp(GetRemoteIpAddress(), cancellationToken);
            var currentCountry = location?.Country;

            if (allowedCountries != null
                &&
                currentCountry != null)
            {
                var tokenizer = new StringTokenizer(allowedCountries, split_characters);

                return tokenizer.Contains(currentCountry, StringSegmentComparer.OrdinalIgnoreCase);
            }

            return false;
        }

        string GetRemoteIpAddress()
        {
            const string HEADER_X_FORWARDED_FOR = "X-Forwarded-For";

            string remoteIpAddress = _httpContextAccessor.HttpContext
                .Connection
                .RemoteIpAddress
                .MapToIPv4()
                .ToString();

            if (_httpContextAccessor.HttpContext
                .Request
                .Headers
                .ContainsKey(HEADER_X_FORWARDED_FOR))
            {
                remoteIpAddress = _httpContextAccessor.HttpContext
                    .Request.Headers[HEADER_X_FORWARDED_FOR];
            }

            return remoteIpAddress;
        }

        async Task<IPApiData> GetLocationFromIp(string ipAddress, CancellationToken cancellationToken = default)
        {
            if (ipAddress == "0.0.0.1")
            {
                ipAddress = "213.97.0.42";
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BASE_ADDRESS}{ipAddress}", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<IPApiData>(
                        utf8Json: stream,
                        options: _serializerOptions,
                        cancellationToken: cancellationToken);
                }
            }


            return default;
        }

        private class IPApiData
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
