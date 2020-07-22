using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.Import
{
    public class ImportProductRequest
        :IRequest<Unit>
    {
        public string Content { get; set; }
    }
}
