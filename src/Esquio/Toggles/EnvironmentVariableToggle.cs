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

        public Task<bool> IsActiveAsync(ToggleExecutionContext  context, CancellationToken cancellationToken = default)
        {
            string environmentVariable = context.Data[EnvironmentVariable]?.ToString();
            string validValues = context.Data[Values]?.ToString();

            string environmentVariableValue = Environment
                .GetEnvironmentVariable(environmentVariable);

            if (environmentVariableValue != null)
            {
                var tokenizer = new StringTokenizer(validValues, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);
                var active = tokenizer.Contains(environmentVariableValue, StringSegmentComparer.OrdinalIgnoreCase);

                return Task.FromResult(active);
            }

            return Task.FromResult(false);
        }
    }
}
