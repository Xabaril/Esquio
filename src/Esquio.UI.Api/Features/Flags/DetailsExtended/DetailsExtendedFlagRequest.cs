using MediatR;

namespace Esquio.UI.Api.Features.Flags.DetailsExtended
{
    public class DetailsExtendedFlagRequest : IRequest<DetailsExtendedFlagResponse>
    {
        public int FeatureId { get; set; }
    }
}
