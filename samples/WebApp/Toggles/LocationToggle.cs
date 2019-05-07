using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Toggles
{
    public class LocationToggle
        : IToggle
    {
        const string Countries = nameof(Countries);

        private readonly IFeatureStore _featureStore;
        private readonly ILocationProviderService _locationProviderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationToggle(IFeatureStore featureStore, IHttpContextAccessor httpContextAccessor, ILocationProviderService locationProviderService)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException();
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _locationProviderService = locationProviderService ?? throw new ArgumentNullException(nameof(locationProviderService));
        }

        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var allowedCountries = toggle.GetParameterValue<string>(Countries);
            var currentCountry = await _locationProviderService
                .GetCountryName(GetRemoteIpAddress());

            if (allowedCountries != null
                &&
                currentCountry != null
                &&
                allowedCountries.Split(';').Contains(currentCountry))
            {
                return true;
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

            if(_httpContextAccessor.HttpContext
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
