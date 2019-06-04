using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.ApiKeys.Details
{
    public class DetailsApiKeyRequestHandler : IRequestHandler<DetailsApiKeyRequest, DetailsApiKeyResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsApiKeyRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }
        public async Task<DetailsApiKeyResponse> Handle(DetailsApiKeyRequest request, CancellationToken cancellationToken)
        {
            var apiKey = await _storeDbContext
               .ApiKeys
               .Where(f => f.Id == request.ApiKeyId)
               .SingleOrDefaultAsync(f => f.Id == request.ApiKeyId, cancellationToken);

            if (apiKey != null)
            {
                return new DetailsApiKeyResponse()
                {
                    Id = apiKey.Id,
                    Name = apiKey.Name,
                    Description = apiKey.Description,
                    Key = apiKey.Key
                };
            }

            return null;
        }
    }
}
