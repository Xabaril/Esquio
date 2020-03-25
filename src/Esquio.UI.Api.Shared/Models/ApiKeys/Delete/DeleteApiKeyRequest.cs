using MediatR;

namespace Esquio.UI.Api.Shared.Models.ApiKeys.Delete
{
    public class DeleteApiKeyRequest : IRequest
    {
        public string Name { get; set; }
    }
}
