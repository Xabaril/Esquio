using MediatR;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequest : IRequest<DetailsFlagResponse>
    {
        public int FeatureId { get; set; }
    }
}
