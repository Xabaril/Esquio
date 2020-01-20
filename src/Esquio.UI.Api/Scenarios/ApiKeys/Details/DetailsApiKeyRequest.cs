using MediatR;

namespace Esquio.UI.Api.Scenarios.ApiKeys.Details
{
    public class DetailsApiKeyRequest : IRequest<DetailsApiKeyResponse>
    {
        public string Name { get; set; }
    }
}
