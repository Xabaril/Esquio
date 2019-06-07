using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that is active depending on current user role name.")]
    [DesignTypeParameter(ParameterName = Roles, ParameterType = "System.String", ParameterDescription = "The collection of rol(es) to activate this toggle separated by ';' character")]
    public class RoleNameToggle
       : IToggle
    {
        internal const string Roles = nameof(Roles);

        private readonly IRoleNameProviderService _roleNameProviderService;
        private readonly IRuntimeFeatureStore _featureStore;

        public RoleNameToggle(IRoleNameProviderService roleNameProviderService, IRuntimeFeatureStore featureStore)
        {
            _roleNameProviderService = roleNameProviderService ?? throw new ArgumentNullException(nameof(roleNameProviderService));
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var currentRole = await _roleNameProviderService
                .GetCurrentRoleNameAsync();

            if (currentRole != null)
            {
                var feature = await _featureStore.FindFeatureAsync(featureName, productName);
                var toggle = feature.GetToggle(this.GetType().FullName);
                var data = toggle.GetData();

                string activeRoles = data.Roles?.ToString();

                if (activeRoles != null)
                {
                    var tokenizer = new StringTokenizer(activeRoles, Globals.DEFAULT_SPLIT_SEPARATOR);

                    return tokenizer.Contains(
                        currentRole, StringSegmentComparer.OrdinalIgnoreCase);
                }
            }
            return false;
        }
    }
}
