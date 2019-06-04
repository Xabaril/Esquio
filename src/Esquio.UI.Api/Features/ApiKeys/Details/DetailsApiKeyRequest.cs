using MediatR;

namespace Esquio.UI.Api.Features.ApiKeys.Details
{
    public class DetailsApiKeyRequest : IRequest<DetailsApiKeyResponse>
    {
        public int ApiKeyId { get; set; }
    }
}
