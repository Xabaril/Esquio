using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on current Environment value.
    /// </summary>
    [DesignType(Description = "Toggle that is active depending on execution environment value.", FriendlyName = "Environment")]
    [DesignTypeParameter(ParameterName = Environments, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection of environments to activate this toggle separated by ';' character")]
    public class EnvironmentToggle
        : IToggle
    {
        private const string Environments = nameof(Environments);

        private readonly IEnvironmentNameProviderService _environmentNameProviderService;
        private readonly IRuntimeFeatureStore _featureStore;

        /// <summary>
        /// Create a new instace of <see cref="EnvironmentToggle"/>.
        /// </summary>
        /// <param name="environmentNameProviderService">The <see cref="IEnvironmentNameProviderService"/> service to be used.</param>
        /// <param name="featureStore">The <see cref="IRuntimeFeatureStore"/> service to be used.</param>
        public EnvironmentToggle(IEnvironmentNameProviderService environmentNameProviderService, IRuntimeFeatureStore featureStore)
        {
            _environmentNameProviderService = environmentNameProviderService ?? throw new ArgumentNullException(nameof(environmentNameProviderService));
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }

        ///  <inheritdoc />
        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore
                .FindFeatureAsync(featureName, productName, cancellationToken);

            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            string environments = data.Environments?.ToString();

            var currentEnvironment = await _environmentNameProviderService
                .GetEnvironmentNameAsync(cancellationToken);

            if (environments != null && currentEnvironment != null)
            {
                var tokenizer = new StringTokenizer(environments, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                return tokenizer.Contains(currentEnvironment, StringSegmentComparer.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
