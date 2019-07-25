using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignType(Description = "Toggle that is active depending on the bucket name created with the value of specified claim.")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = EsquioConstants.PERCENTAGE_PARAMETER_TYPE, ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    [DesignTypeParameter(ParameterName = ClaimType, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The claim type used to get value to rollout.")]
    public class GradualRolloutClaimValueToggle
        : IToggle
    {
        const string NO_CLAIMTYPE_DEFAULT_VALUE = "no-claim-type";

        internal const string ClaimType = nameof(ClaimType);
        internal const string Percentage = nameof(Percentage);
        internal const int Partitions = 100;

        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GradualRolloutClaimValueToggle(IRuntimeFeatureStore featureStore, IHttpContextAccessor httpContextAccessor)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName, cancellationToken);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            if ( Double.TryParse(data.Percentage.ToString(), out double percentage))
            {
                string claimType = data.ClaimType?.ToString();

                if (claimType != null && percentage > 0)
                {
                    var user = _httpContextAccessor
                        .HttpContext
                        .User;

                    if (user != null && user.Identity.IsAuthenticated)
                    {
                        var value = user.FindFirst(claimType)?.Value ?? NO_CLAIMTYPE_DEFAULT_VALUE;

                        var assignedPartition = Partitioner.ResolveToLogicalPartition(value, Partitions);
                        return assignedPartition <= percentage;
                    }
                }
            }

            return false;
        }
    }
}
