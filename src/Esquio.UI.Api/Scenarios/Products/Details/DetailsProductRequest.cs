using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.Details
{
    public class DetailsProductRequest : IRequest<DetailsProductResponse>
    {
        public string ProductName { get; set; }
    }
}
