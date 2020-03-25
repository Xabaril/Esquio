using MediatR;

namespace Esquio.UI.Api.Shared.Models.ApiKeys.List
{
    public class ListApiKeyRequest : IRequest<PaginatedResult<ListApiKeyResponseDetail>>
    {
        public int PageIndex { get; set; } = Constants.Pagination.PageIndex;

        public int PageCount { get; set; } = Constants.Pagination.PageCount;
    }
}
