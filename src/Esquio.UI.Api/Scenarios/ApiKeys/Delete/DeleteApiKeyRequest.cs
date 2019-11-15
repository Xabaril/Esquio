using MediatR;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteApiKeyRequest : IRequest
    {
        public string Name { get; set; }
    }
}
