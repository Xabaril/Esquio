using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Features.Rollback;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Rollback
{
    internal class RollbackFeatureRequestHandler
        : IRequestHandler<RollbackFeatureRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<RollbackFeatureRequestHandler> _logger;

        public RollbackFeatureRequestHandler(StoreDbContext storeDbContext, ILogger<RollbackFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(RollbackFeatureRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Include(f => f.ProductEntity)  // -> this is only needed for "history"
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(cancellationToken);

            var ring = await _storeDbContext
                .Rings
                .Include(r => r.ProductEntity)
                .Where(r => r.Name == request.RingName && r.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync();

            if (feature != null && ring != null)
            {
                var currentState = await _storeDbContext.FeatureStates
                    .Where(fs => fs.FeatureEntityId == feature.Id && fs.RingEntityId == ring.Id)
                    .SingleOrDefaultAsync();

                if (currentState != null)
                {
                    currentState.Enabled = false;
                }
                else
                {
                    _storeDbContext.FeatureStates.Add(new FeatureStateEntity()
                    {
                        RingEntityId = ring.Id,
                        FeatureEntityId = feature.Id,
                        Enabled = false
                    });
                }

                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            //TODO: Improve this log information
            Log.FeatureNotExist(_logger, request.FeatureName.ToString());
            throw new InvalidOperationException("Operation can't be performed because the combination feature product and ring are not valid on this store.");
        }
    }
}
