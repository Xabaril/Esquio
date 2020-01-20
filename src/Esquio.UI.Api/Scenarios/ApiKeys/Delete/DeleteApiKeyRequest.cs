using MediatR;

namespace Esquio.UI.Api.Scenarios.Flags.Delete
{
    public class DeleteApiKeyRequest : IRequest
    {
        public string Name { get; set; }
    }
}
