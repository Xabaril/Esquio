using MediatR;

namespace Esquio.UI.Api.Features.Flags.Update
{
    public class UpdateFlagRequest : IRequest<int>
    {
        public int FlagId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

    }
}
