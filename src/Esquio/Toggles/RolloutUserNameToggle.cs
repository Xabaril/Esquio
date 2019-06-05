using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that is active depending on the bucket name created with user name value and the rollout percentage.")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = "System.Int32", ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    public class RolloutUserNameToggle
        : IToggle
    {
        const string Percentage = nameof(Percentage);
        const string AnonymoysUser = nameof(AnonymoysUser);
        const int Partitions = 10;
        private readonly IUserNameProviderService _userNameProviderService;
        private readonly IRuntimeFeatureStore _featureStore;

        public RolloutUserNameToggle(IUserNameProviderService userNameProviderService, IRuntimeFeatureStore featureStore)
        {
            _userNameProviderService = userNameProviderService ?? throw new System.ArgumentNullException(nameof(userNameProviderService));
            _featureStore = featureStore ?? throw new System.ArgumentNullException(nameof(featureStore));
        }
        public async Task<bool> IsActiveAsync(string featureName, string productName = null)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            int percentage = data.Percentage;

            var currentUserName = await _userNameProviderService
                .GetCurrentUserNameAsync() ?? AnonymoysUser;

            var assignedPartition = Partitioner.ResolveToLogicalPartition(currentUserName, Partitions);

            return assignedPartition <= (Partitions * percentage / 100);
        }
    }
}

