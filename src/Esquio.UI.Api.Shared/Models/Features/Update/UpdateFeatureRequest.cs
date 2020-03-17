using MediatR;

namespace Esquio.UI.Api.Shared.Models.Features.Update
{
    public class UpdateFeatureRequest : IRequest<string>
    {
        internal string CurrentName { get; set; }

        internal string ProductName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

    }
}
