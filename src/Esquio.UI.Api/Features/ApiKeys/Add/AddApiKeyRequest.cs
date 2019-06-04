using MediatR;

namespace Esquio.UI.Api.Features.ApiKeys.Add
{
    public class AddApiKeyRequest : IRequest<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
