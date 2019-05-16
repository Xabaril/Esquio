using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
        private static char[] SPLIT_SEPARATOR = new char[] { ';' };

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRuntimeFeatureStore _featureStore;
        public ClaimValueToggle(IHttpContextAccessor httpContextAccessor, IRuntimeFeatureStore store)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var claimType = toggle.GetParameterValue<string>(ClaimType);
            var allowedValues = toggle.GetParameterValue<string>(ClaimValues);

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
                        var tokenizer = new StringTokenizer(allowedValues, SPLIT_SEPARATOR);

                        return tokenizer.Contains(
                            value, StringSegmentComparer.OrdinalIgnoreCase);
                    }
                }
            }
            return false;
        }
    }
}
