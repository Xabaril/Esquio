using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Toggles.Add
{
    public class AddToggleRequestHandler : IRequestHandler<AddToggleRequest, int>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddToggleRequestHandler> _logger;

        public AddToggleRequestHandler(StoreDbContext storeDbContext, ILogger<AddToggleRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(AddToggleRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Include(t => t.Toggles)
                .Where(t => t.Name == request.FeatureName && t.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var alreadyExistToggleType = feature.Toggles
                    .Any(t => t.Type == request.ToggleType);

                if (!alreadyExistToggleType)
                {
                    var toggle = new ToggleEntity(feature.Id, request.ToggleType);

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
                        foreach (var item in request.Parameters)
                        {
                            toggle.AddOrUpdateParameter(selectedRing, defaultRing, item.Name, item.Value);
                        }

                        feature.Toggles
                            .Add(toggle);

                        await _storeDbContext.SaveChangesAsync(cancellationToken);
                        return toggle.Id;
                    }

                    Log.RingNotExist(_logger, request.RingName, request.ProductName);
                    throw new InvalidOperationException($"Ring {request.RingName} does not exist for product {request.ProductName}.");
                }

                Log.ToggleTypeAlreadyExist(_logger, request.ToggleType, feature.Name);
                throw new InvalidOperationException($"Toggle with type {request.ToggleType} already exist on this feature.");
            }

            Log.FeatureNotExist(_logger, request.FeatureName);
            throw new InvalidOperationException($"The feature {request.FeatureName} does not exist in the store.");
        }
    }
}
