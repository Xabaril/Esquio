using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteApiKeyRequestHandler : IRequestHandler<DeleteApiKeyRequest>
    {
        private readonly StoreDbContext _storeDbContext;

        public DeleteApiKeyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<Unit> Handle(DeleteApiKeyRequest request, CancellationToken cancellationToken)
        {
            var apikey = await _storeDbContext
                .ApiKeys
                .Where(f => f.Id == request.ApiKeyId)
                .SingleOrDefaultAsync(cancellationToken);

            if (apikey != null)
            {
                _storeDbContext.Remove(apikey);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException($"The ApiKey with identifier {request.ApiKeyId} does not exist.");
        }
    }
}
