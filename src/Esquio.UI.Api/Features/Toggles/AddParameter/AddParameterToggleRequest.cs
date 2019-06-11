using MediatR;

namespace Esquio.UI.Api.Features.Toggles.AddParameter
{
    public class AddParameterToggleRequest : IRequest
    {
        internal int ToggleId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string ClrType { get; set; }
    }
}
