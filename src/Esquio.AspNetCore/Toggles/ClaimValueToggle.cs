using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignTypeParameter(ParameterName = ClaimType, ParameterType = "System.String", ParameterDescription = "The claim type used to check value.")]
    [DesignTypeParameter(ParameterName = ClaimValues, ParameterType = "System.String", ParameterDescription = "The claim value to check, multiple items separated by ';'.")]
    public class ClaimValueToggle
        : IToggle
    {
        const string ClaimType = nameof(ClaimType);
        const string ClaimValues = nameof(ClaimValues);
        const char SPLIT_CHARACTER = ';';

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeatureStore _featureStore;
        public ClaimValueToggle(IHttpContextAccessor httpContextAccessor, IFeatureStore store)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var claimType = ((string)await _featureStore
                .GetToggleParameterValueAsync<ClaimValueToggle>(featureName, applicationName, ClaimType));

            var allowedValues = ((string)await _featureStore
                .GetToggleParameterValueAsync<ClaimValueToggle>(featureName, applicationName, ClaimValues));

            if (claimType != null
                &&
                ClaimValues != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    var value = user.FindFirst(claimType)?
                        .Value;

                    if (value != null)
                    {
                        return allowedValues
                           .Split(SPLIT_CHARACTER)
                           .Contains(value, StringComparer.InvariantCultureIgnoreCase);
                    }
                }
            }
            return false;
        }
    }
}
