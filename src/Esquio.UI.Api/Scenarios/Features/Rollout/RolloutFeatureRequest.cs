using MediatR;

namespace Esquio.UI.Api.Scenarios.Flags.Rollout
{
    public class RolloutFeatureRequest
        :IRequest
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}
