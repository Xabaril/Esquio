using MediatR;

namespace Esquio.UI.Api.Features.Flags.Rollout
{
    public class RolloutFlagRequest
        :IRequest
    {
        public int FeatureId { get; set; }
    }
}
