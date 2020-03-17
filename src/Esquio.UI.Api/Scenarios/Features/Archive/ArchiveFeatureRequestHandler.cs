using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Features.Archive;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.Archive
{
    internal class ArchiveFeatureRequestHandler
        : IRequestHandler<ArchiveFeatureRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<ArchiveFeatureRequestHandler> _logger;

        public ArchiveFeatureRequestHandler(StoreDbContext storeDbContext, ILogger<ArchiveFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(ArchiveFeatureRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Archived = true;

                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            Log.FeatureNotExist(_logger, request.FeatureName.ToString());
            throw new InvalidOperationException("Feature does not exist in the store.");
        }
    }
}
