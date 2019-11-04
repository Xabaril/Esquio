using MediatR;

namespace Esquio.UI.Api.Features.Products.Details
{
    public class DetailsProductRequest : IRequest<DetailsProductResponse>
    {
        public string ProductName { get; set; }
    }
}
