using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Store.Details
{
    public class DetailsStoreRequestHandler
        : IRequestHandler<DetailsStoreRequest, DetailsStoreResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DetailsStoreRequestHandler> _logger;

        public DetailsStoreRequestHandler(StoreDbContext storeDbContext, ILogger<DetailsStoreRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DetailsStoreResponse> Handle(DetailsStoreRequest request, CancellationToken cancellationToken)
        {
            var featureEntity = await _storeDbContext
               .Features
               .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
               .Include(f => f.Toggles)
                   .ThenInclude(t => t.Parameters)
               .SingleOrDefaultAsync(cancellationToken);

            if (featureEntity != null)
            {
                return CreateResponse(featureEntity, request.RingName ?? EsquioConstants.DEFAULT_RING_NAME);
            }
            else
            {
                Log.FeatureNotExist(_logger, request.FeatureName);
                return null;
            }
        }

        private DetailsStoreResponse CreateResponse(FeatureEntity featureEntity, string ringName)
        {
            var feature = new DetailsStoreResponse
            {
                FeatureName = featureEntity.Name,
                Enabled = featureEntity.Enabled
            };

            foreach (var toggleConfiguration in featureEntity.Toggles)
            {
                var type = toggleConfiguration.Type;
                var parameters = new Dictionary<string, object>();

                var groupingParameters = toggleConfiguration.Parameters
                    .GroupBy(g => g.RingName);

                var defaultRingParameters = groupingParameters
                    .Where(g => g.Key == EsquioConstants.DEFAULT_RING_NAME)
                    .SingleOrDefault();

                if (defaultRingParameters != null
                    &&
                    defaultRingParameters.Any())
                {
                    foreach (var item in defaultRingParameters)
                    {
                        parameters.Add(item.Name, item.Value);
                    }
                }

                if (ringName != EsquioConstants.DEFAULT_RING_NAME)
                {
                    var selectedRingParameters = groupingParameters
                        .Where(g => g.Key == ringName)
                        .SingleOrDefault();

                    if (selectedRingParameters != null
                        &&
                        selectedRingParameters.Any())
                    {
                        foreach (var item in selectedRingParameters)
                        {
                            if ( parameters.ContainsKey(item.Name))
                            {
                                parameters[item.Name] = item.Value;
                            }
                        }
                    }
                }

                feature.Toggles.TryAdd(type, parameters);
            }

            return feature;
        }
    }
}
