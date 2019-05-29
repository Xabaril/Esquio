using MediatR;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequest : IRequest<DetailsFlagResponse>
    {
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
    }
}
