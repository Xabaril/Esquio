using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that is active depending on the bucket name created with user name value and the rollout percentage.")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = EsquioConstants.PERCENTAGE_PARAMETER_TYPE, ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    public class GradualRolloutUserNameToggle
        : IToggle
    {
        internal const string Percentage = nameof(Percentage);
        internal const string AnonymousUser = nameof(AnonymousUser);
        internal const int Partitions = 100;

        private readonly IUserNameProviderService _userNameProviderService;
        private readonly IRuntimeFeatureStore _featureStore;

        public GradualRolloutUserNameToggle(IUserNameProviderService userNameProviderService, IRuntimeFeatureStore featureStore)
        {
            _userNameProviderService = userNameProviderService ?? throw new System.ArgumentNullException(nameof(userNameProviderService));
            _featureStore = featureStore ?? throw new System.ArgumentNullException(nameof(featureStore));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName, cancellationToken);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            if (Double.TryParse(data.Percentage.ToString(), out double percentage))
            {
                if (percentage > 0)
                {
                    var currentUserName = await _userNameProviderService
                        .GetCurrentUserNameAsync() ?? AnonymousUser;

                    var assignedPartition = Partitioner.ResolveToLogicalPartition(currentUserName, Partitions);

                    return assignedPartition <= percentage;
                }
            }

            return false;
        }
    }
}

