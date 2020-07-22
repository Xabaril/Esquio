using MediatR;

namespace Esquio.UI.Api.Shared.Models.Products.Export
{
    public class ExportProductRequest
        :IRequest<ExportProductResponse>
    {
        public string ProductName { get; set; }
    }
}
