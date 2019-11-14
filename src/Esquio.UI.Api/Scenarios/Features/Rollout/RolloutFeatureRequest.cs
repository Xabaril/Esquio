using MediatR;

namespace Esquio.UI.Api.Features.Flags.Rollout
{
    public class RolloutFeatureRequest
        :IRequest
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}
