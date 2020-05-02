using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.Delete
{
    public class DeleteProductRequest : IRequest
    {
        public string ProductName { get; set; }
    }
}
