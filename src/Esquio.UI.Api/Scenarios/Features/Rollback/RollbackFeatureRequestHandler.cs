using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
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
                .Include(f => f.ProductEntity) //-> this is only needed for "history"
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Enabled = false;

                if (feature.Toggles.Any())
                {
                    feature.Toggles.Clear();

                }

                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            Log.FeatureNotExist(_logger, request.FeatureName.ToString());
            throw new InvalidOperationException("Feature does not exist in the store.");
        }
    }
}
