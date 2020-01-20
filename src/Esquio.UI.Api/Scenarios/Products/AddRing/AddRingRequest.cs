using MediatR;

namespace Esquio.UI.Api.Scenarios.Products.AddRing
{
    public class AddRingRequest : IRequest<Unit>
    {
        internal string ProductName { get; set; }
        public string Name { get; set; }
    }
}
