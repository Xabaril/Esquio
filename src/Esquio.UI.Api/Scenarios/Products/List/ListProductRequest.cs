using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.List
{
    public class ListProductRequest : IRequest<ListProductResponse>
    {
        public int PageIndex { get; set; } = ApiConstants.Pagination.PageIndex;

        public int PageCount { get; set; } = ApiConstants.Pagination.PageCount;
    }
}
