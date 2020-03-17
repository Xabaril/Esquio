using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.List
{
    public class ListProductRequest : IRequest<PaginatedResult<ListProductResponseDetail>>
    {
        public int PageIndex { get; set; } = Constants.Pagination.PageIndex;

        public int PageCount { get; set; } = Constants.Pagination.PageCount;
    }
}
