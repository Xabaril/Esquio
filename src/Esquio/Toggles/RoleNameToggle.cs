using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = Roles, ParameterType = "System.String", ParameterDescription = "The collection of rol(es) to activate this toggle separated by ';' character")]
    public class RoleNameToggle
       : IToggle
    {
        const string SPLIT_SEPARATOR = ";";
        const string Roles = nameof(Roles);

        public async Task<bool> IsActiveAsync(IFeatureContext context)
        {
            var roleNameProviderService = context.ServiceProvider.GetService<IRoleNameProviderService>();
            var featureStore = context.ServiceProvider.GetService<IFeatureStore>();

            var currentRole = await roleNameProviderService
                .GetCurrentRoleNameAsync();

            if (currentRole != null)
            {
                var activeRoles = (string)await featureStore
                    .GetToggleParameterValueAsync<UserNameToggle>(context.ApplicationName, context.FeatureName, Roles);

                if (activeRoles != null &&
                    activeRoles.Split(SPLIT_SEPARATOR).Contains(currentRole, StringComparer.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
