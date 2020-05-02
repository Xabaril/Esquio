using MediatR;

namespace Esquio.UI.Api.Shared.Models.Audit.List
{
    public class ListAuditRequest : IRequest<PaginatedResult<ListAuditResponseDetail>>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;
    }
}
