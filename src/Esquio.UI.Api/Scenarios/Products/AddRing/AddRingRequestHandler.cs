using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Products.AddRing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.AddRing
{
    public class AddRingRequestHandler : IRequestHandler<AddRingRequest, Unit>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddRingRequestHandler> _logger;

        public AddRingRequestHandler(StoreDbContext storeDbContext, ILogger<AddRingRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(AddRingRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .Products
                .Where(p => p.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (existing != null)
            {
                var existingRing = await _storeDbContext
                    .Rings
                    .Where(r => r.Name == request.Name && r.Product.Name == request.ProductName)
                    .SingleOrDefaultAsync(cancellationToken);

                if (existingRing == null)
                {
                    var ring = new RingEntity(
                        productEntityId: existing.Id,
                        name: request.Name,
                        byDefault: false);

                    _storeDbContext.Rings
                        .Add(ring);

                    await _storeDbContext.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                else
                {
                    Log.RingAlreadyExist(_logger, request.Name, request.ProductName);
                    throw new InvalidOperationException($"The ring {request.Name} already exist for product {request.ProductName}");
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
