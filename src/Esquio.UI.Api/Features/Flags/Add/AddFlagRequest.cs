using MediatR;

namespace Esquio.UI.Api.Features.Flags.Add
{
    public class AddFlagRequest : IRequest<int>
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

    }
}
