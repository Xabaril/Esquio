using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = Users, ParameterType = "System.String", ParameterDescription = "The collection of user(s) to activate this toggle separated by ';' character")]
    public class UserNameToggle
        : IToggle
    {
        const string SPLIT_SEPARATOR = ";";
        const string Users = nameof(Users);

        public async Task<bool> IsActiveAsync(IFeatureContext context)
        {
            var userNameProviderService = context.ServiceProvider.GetService<IUserNameProviderService>();
            var featureStore = context.ServiceProvider.GetService<IFeatureStore>();

            var currentUserName = await userNameProviderService
                .GetCurrentUserNameAsync();

            if (currentUserName != null)
            {
                var activeUserNames = (string)await featureStore
                    .GetToggleParameterValueAsync<UserNameToggle>(context.ApplicationName, context.FeatureName, Users);

                if (activeUserNames != null &&
                    activeUserNames.Split(SPLIT_SEPARATOR).Contains(currentUserName, StringComparer.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}