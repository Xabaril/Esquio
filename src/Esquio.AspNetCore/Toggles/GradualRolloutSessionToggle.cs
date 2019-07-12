using Esquio.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Toggles
{
    [DesignType(Description = "Toggle that is active depending on the session identifier bucket and the percentage selected.")]
    [DesignTypeParameter(ParameterName = Percentage, ParameterType = "System.Int32", ParameterDescription = "The percentage of sessions that activate this toggle. Percentage from 0 to 100.")]
    public class GradualRolloutSessionToggle
        : IToggle
    {
        internal const string Percentage = nameof(Percentage);
        internal const int Partitions = 100;

        private readonly IRuntimeFeatureStore _featureStore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GradualRolloutSessionToggle> _logger;

        public GradualRolloutSessionToggle(IRuntimeFeatureStore featureStore, IHttpContextAccessor httpContextAccessor, ILogger<GradualRolloutSessionToggle> logger)
        {
            _featureStore = featureStore ?? throw new ArgumentNullException(nameof(featureStore));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> IsActiveAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            var feature = await _featureStore.FindFeatureAsync(featureName, productName, cancellationToken);
            var toggle = feature.GetToggle(this.GetType().FullName);
            var data = toggle.GetData();

            if (Double.TryParse(data.Percentage.ToString(), out double percentage))
            {
                if (percentage > 0d)
                {
                    try
                    {
                        var sessionId = _httpContextAccessor
                            .HttpContext
                            .Session
                            .Id;

                        var assignedPartition = Partitioner.ResolveToLogicalPartition(sessionId, Partitions);
                        return assignedPartition <= percentage;
                    }
                    catch (InvalidOperationException)
                    {
                        _logger.LogError($"The toggle {nameof(GradualRolloutSessionToggle)} can't perform rollout on Session because  Session has not been configured for this application or request.");
                    }
                }
            }

            return false;
        }
    }
}
