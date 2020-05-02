using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.Rollout
{
    public class RolloutFeatureRequest
        :IRequest
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}
