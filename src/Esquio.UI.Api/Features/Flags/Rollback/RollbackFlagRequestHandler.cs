using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Rollback
{
    internal class RollbackFlagRequestHandler
        : IRequestHandler<RollbackFlagRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<RollbackFlagRequestHandler> _logger;

        public RollbackFlagRequestHandler(StoreDbContext storeDbContext, ILogger<RollbackFlagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(RollbackFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
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
