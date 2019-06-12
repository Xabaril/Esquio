using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Toggles
{
    [DesignType(Description = "Toggle that is active depending request ip location.")]
    [DesignTypeParameter(ParameterName = Countries, ParameterType = "System.String", ParameterDescription = "Collection of countries delimited by ';' character.")]
    public class LocationToggle
        : IToggle
    {
        const string Countries = nameof(Countries);

        static char[] split_characters = new char[] { ';' };

        private readonly IRuntimeFeatureStore _featureStore;
        private readonly ILocationProviderService _locationProviderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationToggle(IRuntimeFeatureStore featureStore, IHttpContextAccessor httpContextAccessor, ILocationProviderService locationProviderService)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException();
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _locationProviderService = locationProviderService ?? throw new ArgumentNullException(nameof(locationProviderService));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            string allowedCountries = data.Countries;
            var currentCountry = await _locationProviderService
                .GetCountryName(GetRemoteIpAddress());

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
    }
}
