using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.Add
{
    public class AddProductRequest : IRequest<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultRingName { get; set; } = EsquioConstants.DEFAULT_RING_NAME;
    }
}
