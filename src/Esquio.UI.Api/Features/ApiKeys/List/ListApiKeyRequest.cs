using MediatR;

namespace Esquio.UI.Api.Features.ApiKeys.List
{
    public class ListApiKeyRequest : IRequest<ListApiKeyResponse>
    {
        public int PageIndex { get; set; } = 0;

        public int PageCount { get; set; } = 10;
    }
}
