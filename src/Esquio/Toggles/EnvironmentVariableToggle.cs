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
    [DesignType(Description = "The value of the configured environment variable is in the list.", FriendlyName = "Environment Variable")]
    [DesignTypeParameter(ParameterName = EnvironmentVariable, ParameterType = EsquioConstants.STRING_PARAMETER_TYPE, ParameterDescription = "The environment variable name.")]
    [DesignTypeParameter(ParameterName = Values, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The values to activate this toggle separated by ';' character.")]
    public class EnvironmentVariableToggle
        : IToggle
    {
        private const string EnvironmentVariable = nameof(EnvironmentVariable);
        private const string Values = nameof(Values);

        /// <inheritdoc/>
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext  context, CancellationToken cancellationToken = default)
        {
            string environmentVariable = context.Data[EnvironmentVariable]?.ToString();
            string validValues = context.Data[Values]?.ToString();

            string environmentVariableValue = Environment
                .GetEnvironmentVariable(environmentVariable);

            if (environmentVariableValue != null)
            {
                var tokenizer = new StringTokenizer(validValues, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);
                var active = tokenizer.Contains(environmentVariableValue, StringSegmentComparer.OrdinalIgnoreCase);

                return new ValueTask<bool>(active);
            }

            return new ValueTask<bool>(false);
        }
    }
}
