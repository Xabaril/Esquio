using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    [DesignTypeParameter(ParameterName = Environments, ParameterType = "System.String", ParameterDescription = "The collection of environments to activate this toggle separated by ';' character")]
    public class EnvironmentToggle
        : IToggle
    {
        private const string Environments = nameof(Environments);
        const string SPLIT_SEPARATOR = ";";

        private readonly IEnvironmentNameProviderService _environmentNameProviderService;
        private readonly IFeatureStore _featureStore;
        public EnvironmentToggle(IEnvironmentNameProviderService environmentNameProviderService, IFeatureStore featureStore)
        {
            _environmentNameProviderService = environmentNameProviderService ?? throw new ArgumentNullException(nameof(environmentNameProviderService));
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }
        public async Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, applicationName);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var environments = toggle.GetParameterValue<string>(Environments);

            var currentEnvironment = await _environmentNameProviderService
                .GetEnvironmentNameAsync();

            if (environments != null && currentEnvironment != null)
            {
                return environments.Split(SPLIT_SEPARATOR)
                    .Contains(currentEnvironment, StringComparer.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
