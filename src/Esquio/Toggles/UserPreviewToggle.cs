using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = PreviewUsers, ParameterType = "System.String", ParameterDescription = "The collection of preview user(s) that can active this toggle separated  by ';' character")]
    [DesignTypeParameter(ParameterName = EnabledUsers, ParameterType = "System.String", ParameterDescription = "The collection of preview enabled user(s) with this toggle active separated by ';' character")]
    public class UserPreviewToggle
         : IToggle
    {
        const char SPLIT_SEPARATOR = ';';
        const string PreviewUsers = nameof(PreviewUsers);
        const string EnabledUsers = nameof(EnabledUsers);

        private readonly IUserNameProviderService _userNameProviderService;
        private readonly IFeatureStore _featureStore;

        public UserPreviewToggle(IUserNameProviderService userNameProviderService, IFeatureStore featureStore)
        {
            _userNameProviderService = userNameProviderService ?? throw new ArgumentNullException(nameof(userNameProviderService));
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var currentUserName = await _userNameProviderService
                .GetCurrentUserNameAsync();

            if (currentUserName != null)
            {
                var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
                var toggle = feature.GetToggle(this.GetType().FullName);
                var previewUsers = toggle.GetParameterValue<string>(PreviewUsers);
                var enabledUsers = toggle.GetParameterValue<string>(EnabledUsers);

                if (previewUsers != null 
                    &&
                    previewUsers.Split(SPLIT_SEPARATOR).Contains(currentUserName, StringComparer.InvariantCultureIgnoreCase)
                    &&
                    enabledUsers != null
                    &&
                    enabledUsers.Split(SPLIT_SEPARATOR).Contains(currentUserName, StringComparer.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
