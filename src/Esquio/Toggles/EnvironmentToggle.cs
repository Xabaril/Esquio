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
    [DesignType(Description = "Toggle that is active depending on execution environment value.", FriendlyName = "On Environment")]
    [DesignTypeParameter(ParameterName = Environments, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection of environments to activate this toggle separated by ';' character")]
    public class EnvironmentToggle
        : IToggle
    {
        private const string Environments = nameof(Environments);

        private readonly IEnvironmentNameProviderService _environmentNameProviderService;

        /// <summary>
        /// Create a new instace of <see cref="EnvironmentToggle"/>.
        /// </summary>
        /// <param name="environmentNameProviderService">The <see cref="IEnvironmentNameProviderService"/> service to be used.</param>
        public EnvironmentToggle(IEnvironmentNameProviderService environmentNameProviderService)
        {
            _environmentNameProviderService = environmentNameProviderService ?? throw new ArgumentNullException(nameof(environmentNameProviderService));
        }

        ///  <inheritdoc />
        public async Task<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            string environments = context.Data[Environments]?.ToString();

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
