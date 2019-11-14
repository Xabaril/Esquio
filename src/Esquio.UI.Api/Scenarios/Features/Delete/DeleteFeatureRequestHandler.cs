using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFeatureRequestHandler : IRequestHandler<DeleteFeatureRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DeleteFeatureRequestHandler> _logger;

        public DeleteFeatureRequestHandler(StoreDbContext storeDbContext, ILogger<DeleteFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteFeatureRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                _storeDbContext.Remove(feature);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException("Feature does not exist in the store.");
        }
    }
}
