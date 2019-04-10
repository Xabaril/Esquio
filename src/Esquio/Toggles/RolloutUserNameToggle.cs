using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = "System.Int32", ParameterDescription = "The percentage of users that activate this toggle. Percentage from 0 to 100.")]
    public class RolloutUserNameToggle
        : IToggle
    {
        const string Percentage = nameof(Percentage);
        const string AnonymoysUser = nameof(AnonymoysUser);
        const int Partitions = 10;

        public async Task<bool> IsActiveAsync(IFeatureContext context)
        {
            var userNameProviderService = context.ServiceProvider.GetService<IUserNameProviderService>();
            var featureStore = context.ServiceProvider.GetService<IFeatureStore>();

            var percentage = (int)await featureStore
                  .GetParameterValueAsync<RolloutUserNameToggle>(context.ApplicationName, context.FeatureName, Percentage);

            var currentUserName = await userNameProviderService
                .GetCurrentUserNameAsync() ?? AnonymoysUser;

            var assignedPartition = Partitioner.ResolveToLogicalPartition(currentUserName, Partitions);

            return assignedPartition <= ((Partitions * percentage) / 100);
        }
    }
}

