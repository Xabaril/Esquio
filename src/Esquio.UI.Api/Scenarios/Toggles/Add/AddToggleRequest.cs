using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Toggles.Add
{
    public class AddToggleRequest : IRequest<int>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string ToggleType { get; set; }

        public List<AddToggleRequestDetailParameter> Parameters { get; set; } = new List<AddToggleRequestDetailParameter>();
    }

    public class AddToggleRequestDetailParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }
    }
}
