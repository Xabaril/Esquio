using MediatR;

namespace Esquio.UI.Api.Scenarios.Flags.Update
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
