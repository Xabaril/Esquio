using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on the current value for the specified claim on ClaimType property.
    /// </summary>
    [DesignType(Description = "Toggle that is active depending on the current claims of authenticated users.", FriendlyName = "Identity Claim Value")]
    [DesignTypeParameter(ParameterName = ClaimType, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The claim type used to check value.")]
    [DesignTypeParameter(ParameterName = ClaimValues, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The claim value to check, multiple items separated by ';'.")]
    public class ClaimValueToggle
        : IToggle
    {
        internal const string ClaimType = nameof(ClaimType);
        internal const string ClaimValues = nameof(ClaimValues);

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRuntimeFeatureStore _featureStore;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="featureStore">The <see cref="IRuntimeFeatureStore"/> service to be used.</param>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> service to be used.</param>
        public ClaimValueToggle(IRuntimeFeatureStore featureStore, IHttpContextAccessor httpContextAccessor)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        ///<inheritdoc/>
        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore
                .FindFeatureAsync(featureName, productName, cancellationToken);

            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            string claimType = data.ClaimType?.ToString();
            string allowedValues = data.ClaimValues?.ToString();

            if (claimType != null
                &&
                allowedValues != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (user != null && user.Identity.IsAuthenticated)
                {

                    var claimValues = user.Claims
                        .Where(claim => claim.Type == claimType)
                        .Select(claim => claim.Value);

                    foreach (var item in claimValues)
                    {
                        if (item != null)
                        {
                            var tokenizer = new StringTokenizer(allowedValues, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                            if (tokenizer.Contains(item, StringSegmentComparer.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
