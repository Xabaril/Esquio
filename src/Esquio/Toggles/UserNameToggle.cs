using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignType(Description = "Toggle that is active depending on current user name.")]
    [DesignTypeParameter(ParameterName = Users, ParameterType = "System.String", ParameterDescription = "The collection of user(s) to activate this toggle separated by ';' character")]
    public class UserNameToggle
        : IToggle
    {
        const string Users = nameof(Users);

        private readonly IUserNameProviderService _userNameProviderService;
        private readonly IRuntimeFeatureStore _featureStore;

        public UserNameToggle(IUserNameProviderService userNameProviderService, IRuntimeFeatureStore featureStore)
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
                var data = toggle.GetData();

                string activeUserNames = data.Users?.ToString();

                if (activeUserNames != null)
                {
                    var tokenizer = new StringTokenizer(activeUserNames, Globals.DEFAULT_SPLIT_SEPARATOR);

                    return tokenizer.Contains(
                        currentUserName, StringSegmentComparer.OrdinalIgnoreCase);
                }
            }
            return false;
        }
    }
}