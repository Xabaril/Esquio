using MediatR;

namespace Esquio.UI.Api.Shared.Models.ApiKeys.Details
{
    public class DetailsApiKeyRequest : IRequest<DetailsApiKeyResponse>
    {
        public string Name { get; set; }
    }
}
