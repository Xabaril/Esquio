using MediatR;

namespace Esquio.UI.Api.Features.Products.Delete
{
    public class DeleteProductRequest : IRequest
    {
        public string ProductName { get; set; }
    }
}
