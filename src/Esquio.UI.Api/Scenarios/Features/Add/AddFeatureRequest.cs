using MediatR;

namespace Esquio.UI.Api.Features.Flags.Add
{
    public class AddFeatureRequest : IRequest<string>
    {
        internal string ProductName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }
}
