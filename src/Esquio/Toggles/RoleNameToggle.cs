using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = Roles, ParameterType = "System.String", ParameterDescription = "The collection of rol(es) to activate this toggle separated by ';' character")]
    public class RoleNameToggle
       : IToggle
    {
        static char[] SPLIT_SEPARATOR = new char[] { ';' };
        const string Roles = nameof(Roles);
        private readonly IRoleNameProviderService _roleNameProviderService;
        private readonly IFeatureStore _featureStore;

        public RoleNameToggle(IRoleNameProviderService roleNameProviderService, IFeatureStore featureStore)
        {
            _roleNameProviderService = roleNameProviderService ?? throw new ArgumentNullException(nameof(roleNameProviderService));
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var currentRole = await _roleNameProviderService
                .GetCurrentRoleNameAsync();

            if (currentRole != null)
            {
                var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
                var toggle = feature.GetToggle(this.GetType().FullName);
                var activeRoles = toggle.GetParameterValue<string>(Roles);

                if (activeRoles != null)
                {
                    var tokenizer = new StringTokenizer(activeRoles, SPLIT_SEPARATOR);

                    return tokenizer.Contains(
                        currentRole, StringSegmentComparer.OrdinalIgnoreCase);
                }
            }

            return false;
        }
    }
}
