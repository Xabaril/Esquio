using MediatR;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequest : IRequest<DetailsFlagResponse>
    {
        public int ProductId { get; set; }
        public int FlagId { get; set; }
    }
}
