using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Features.State;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Features.State
{
    public class StateFeatureRequestHandler
        : IRequestHandler<StateFeatureRequest, StateFeatureResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<StateFeatureRequestHandler> _logger;

        public StateFeatureRequestHandler(StoreDbContext storeDbContext,ILogger<StateFeatureRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<StateFeatureResponse> Handle(StateFeatureRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
               .Features
               .Where(f => f.ProductEntity.Name == request.ProductName && f.Name == request.FeatureName)
               .Include(f => f.FeatureStates)
               .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                var rings = await _storeDbContext
                  .Deployments
                  .Where(r => r.ProductEntityId == feature.ProductEntityId)
                  .ToListAsync();

                return new StateFeatureResponse()
                {
                    States = rings.Select(r => new StateFeatureResponseDetail()
                    {
                        RingName = r.Name,
                        Default = r.ByDefault,
                        Enabled = feature.FeatureStates.Where(fs => fs.RingEntityId == r.Id).SingleOrDefault()?.Enabled ?? false
                    }).ToList()
                };
            }

            Log.FeatureNotExist(_logger, request.FeatureName.ToString());
            return null;
        }
    }
}
