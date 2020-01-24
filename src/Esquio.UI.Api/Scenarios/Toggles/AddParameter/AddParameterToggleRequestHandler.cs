using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles.AddParameter
{
    public class AddParameterToggleRequestHandler : IRequestHandler<AddParameterToggleRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddParameterToggleRequestHandler> _logger;

        public AddParameterToggleRequestHandler(StoreDbContext storeDbContext, ILogger<AddParameterToggleRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(AddParameterToggleRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Include(f => f.Toggles)
                    .ThenInclude(t => t.Parameters)
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var toggle = feature.Toggles
                    .Where(t => t.Type == request.ToggleType)
                    .SingleOrDefault();

                if (toggle != null)
                {
                    var allowedRings = await _storeDbContext
                        .Rings
                        .Where(p => p.ProductEntityId == feature.ProductEntityId)
                        .ToListAsync();

                    var defaultRing = allowedRings
                           .Where(r => r.ByDefault)
                           .SingleOrDefault();

                    var selectedRing = defaultRing;

                    if (!string.IsNullOrEmpty(request.RingName))
                    {
                        selectedRing = allowedRings
                           .Where(r => r.Name == request.RingName)
                           .SingleOrDefault();
                    }

                    if (selectedRing != null)
                    {
                        toggle.AddOrUpdateParameter(selectedRing, defaultRing, request.Name, request.Value);

                        await _storeDbContext.SaveChangesAsync(cancellationToken);
                        return Unit.Value;
                    }

                    Log.RingNotExist(_logger, request.RingName,request.ProductName);
                    throw new InvalidOperationException($"Ring {request.RingName} does not exist for product {request.ProductName}.");
                }

                Log.ToggleNotExist(_logger, request.ToggleType);
                throw new InvalidOperationException($"Toggle {request.ToggleType} does not exist on feature {request.FeatureName} for product {request.ProductName}.");
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException($"Feature {request.FeatureName} does not exist on product {request.ProductName}.");
        }
    }
}
