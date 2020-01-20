using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Delete
{
    public class DeleteApiKeyRequestHandler : IRequestHandler<DeleteApiKeyRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteApiKeyRequestHandler> _logger;

        public DeleteApiKeyRequestHandler(StoreDbContext storeDbContext,ILogger<DeleteApiKeyRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteApiKeyRequest request, CancellationToken cancellationToken)
        {
            var apikey = await _storeDbContext
                .ApiKeys
                .Where(f => f.Name == request.Name)
                .SingleOrDefaultAsync(cancellationToken);

            if (apikey != null)
            {
                _storeDbContext.Remove(apikey);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.ApiKeyNotExist(_logger, request.Name);
            throw new InvalidOperationException($"The ApiKey with identifier {request.Name} does not exist and can't be deleted.");
        }
    }
}
