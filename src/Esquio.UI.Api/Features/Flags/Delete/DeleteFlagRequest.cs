using MediatR;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFlagRequest : IRequest
    {
        public int ProductId { get; set; }
        public int FlagId { get; set; }
    }
}
