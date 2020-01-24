using MediatR;

namespace Esquio.UI.Api.Features.Audit.List
{
    public class ListAuditRequest : IRequest<ListAuditResponse>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;
    }
}
