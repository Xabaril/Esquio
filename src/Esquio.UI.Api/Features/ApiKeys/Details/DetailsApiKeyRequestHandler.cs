using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.ApiKeys.Details
{
    public class DetailsApiKeyRequestHandler : IRequestHandler<DetailsApiKeyRequest, DetailsApiKeyResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsApiKeyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<DetailsApiKeyResponse> Handle(DetailsApiKeyRequest request, CancellationToken cancellationToken)
        {
            var apiKey = await _storeDbContext
               .ApiKeys
               .Where(f => f.Id == request.ApiKeyId)
               .SingleOrDefaultAsync(cancellationToken);

            if (apiKey != null)
            {
                return new DetailsApiKeyResponse()
                {
                    Id = apiKey.Id,
                    Name = apiKey.Name,
                    ValidTo = apiKey.ValidTo,
                };
            }

            return null;
        }
    }
}
