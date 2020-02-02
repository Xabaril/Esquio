using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.DeleteRing
{
    public class DeleteRingRequestHandler : IRequestHandler<DeleteRingRequest, Unit>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteRingRequestHandler> _logger;

        public DeleteRingRequestHandler(StoreDbContext storeDbContext, ILogger<DeleteRingRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteRingRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .Products
                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                var existingRing = await _storeDbContext
                    .Rings
                    .Where(r => r.Name == request.RingName)
                    .SingleOrDefaultAsync(cancellationToken);

                if (existingRing != null)
                {
                    if(existingRing.ByDefault)
                    {
                        Log.CantDeleteDefaultRing(_logger, request.RingName, request.ProductName);
                        throw new InvalidOperationException($"The ring {request.RingName} is default ring for product {request.ProductName} and can't be deleted.");
                    }
                    
                    _storeDbContext.Rings
                        .Remove(existingRing);

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                else
                {
                    Log.RingNotExist(_logger, request.RingName, request.ProductName);
                    throw new InvalidOperationException($"The ring {request.RingName} does not exist for product {request.ProductName}");
                }
            }
            else
            {
                Log.ProductNotExist(_logger, request.ProductName);
                throw new InvalidOperationException($"The product with id {request.ProductName} does not exist in the store.");
            }
        }
    }
}
