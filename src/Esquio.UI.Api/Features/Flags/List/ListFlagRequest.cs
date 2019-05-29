using MediatR;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFlagRequest : IRequest<ListFlagResponse>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;

        public int ProductId { get; set; }
    }
}
