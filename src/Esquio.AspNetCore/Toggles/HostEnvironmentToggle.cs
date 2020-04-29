using Esquio.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on current Environment value.
    /// </summary>
    [DesignType(Description = "The host execution environment and its value is in the list.", FriendlyName = "On Environment")]
    [DesignTypeParameter(ParameterName = Environments, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection of environments to activate this toggle separated by ';' character")]
    public class HostEnvironmentToggle
        : IToggle
    {
        private const string Environments = nameof(Environments);

        private readonly IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Create a new instace of <see cref="EnvironmentToggle"/>.
        /// </summary>
        /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/> service to be used.</param>
        public HostEnvironmentToggle(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        ///  <inheritdoc />
        public ValueTask<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            string environments = context.Data[Environments]?.ToString();

            if (environments != null)
            {
                var tokenizer = new StringTokenizer(environments, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);
                var isActive = tokenizer.Contains(_hostEnvironment.EnvironmentName, StringSegmentComparer.OrdinalIgnoreCase);

                return new ValueTask<bool>(isActive);
            }

            return new ValueTask<bool>(false);
        }
    }
}
