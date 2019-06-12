using MediatR;

namespace Esquio.UI.Api.Features.Products.List
{
    public class ListProductRequest : IRequest<ListProductResponse>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;
    }
}
