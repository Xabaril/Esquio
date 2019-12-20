using Esquio.Abstractions;
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
    [DesignType(Description = "Toggle that is active depending on value of specified environment variable.", FriendlyName = "By Environment Variable")]
    [DesignTypeParameter(ParameterName = EnvironmentVariable, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The environment variable name to observe.")]
    [DesignTypeParameter(ParameterName = Values, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection environment variable valid values to activate this toggle separated by ';' character.")]
    public class EnvironmentVariableToggle
        : IToggle
    {
        private const string EnvironmentVariable = nameof(EnvironmentVariable);
        private const string Values = nameof(Values);

        private readonly IRuntimeFeatureStore _featureStore;

        public EnvironmentVariableToggle(IRuntimeFeatureStore featureStore)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName, cancellationToken);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            string environmentVariable = data.EnvironmentVariable
                .ToString();

            string validValues = data.Values
                .ToString();

            string environmentVariableValue = Environment
                .GetEnvironmentVariable(environmentVariable);

            if ( environmentVariableValue != null )
            {
                var tokenizer = new StringTokenizer(validValues, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                return tokenizer.Contains(environmentVariableValue, StringSegmentComparer.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
