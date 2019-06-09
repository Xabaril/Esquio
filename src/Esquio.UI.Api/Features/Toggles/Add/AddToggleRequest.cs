using MediatR;
using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Toggles.Add
{
    public class AddToggleRequest : IRequest<int>
    {
        public int FeatureId { get; set; }

        public string ToggleType { get; set; }

        public List<AddToggleRequestDetailParameter> Parameters { get; set; }
    }

    public class AddToggleRequestDetailParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string ClrType { get; set; }
    }
}
