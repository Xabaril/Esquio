using MediatR;

namespace Esquio.UI.Api.Features.Toggles.Parameter
{
    public class ParameterToggleRequest : IRequest
    {
        public int ToogleId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
