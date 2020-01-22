using MediatR;

namespace Esquio.UI.Api.Scenarios.Toggles.AddParameter
{
    public class AddParameterToggleRequest : IRequest
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string RingName { get; set; }

        public string ToggleType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
