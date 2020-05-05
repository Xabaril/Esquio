using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Metrics;
using Esquio.UI.Api.Shared.Models.Configuration.Details;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Configuration.Details
{
    public class DetailsConfigurationRequestHandler
        : IRequestHandler<DetailsConfigurationRequest, DetailsConfigurationResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DetailsConfigurationRequestHandler> _logger = null;
        private readonly EsquioMetricsClient _metricsClient;

        public DetailsConfigurationRequestHandler(StoreDbContext storeDbContext, ILogger<DetailsConfigurationRequestHandler> logger, EsquioMetricsClient metricsClient = null)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _metricsClient = metricsClient;
        }

        public async Task<DetailsConfigurationResponse> Handle(DetailsConfigurationRequest request, CancellationToken cancellationToken)
        {
            var featureEntity = await _storeDbContext
               .Features
               .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
               .Include(f=> f.FeatureStates)
               .Include(f => f.Toggles)
                   .ThenInclude(t => t.Parameters)
               .SingleOrDefaultAsync(cancellationToken);

            DetailsConfigurationResponse response = null;
            ConfigurationRequestMetric metric = null;

            if (featureEntity != null)
            {
                metric = ConfigurationRequestMetric
                    .FromSuccess(request.ProductName, request.FeatureName, request.RingName ?? EsquioConstants.DEFAULT_RING_NAME);

                response = CreateResponse(featureEntity, request.RingName ?? EsquioConstants.DEFAULT_RING_NAME);
            }
            else
            {
                metric = ConfigurationRequestMetric
                    .FromNotFound(request.ProductName, request.FeatureName, request.RingName ?? EsquioConstants.DEFAULT_RING_NAME);

                Log.FeatureNotExist(_logger, request.FeatureName);
            }

            if (_metricsClient != null)
            {
                _metricsClient.Add(metric);
            }

            return response;
        }

        private DetailsConfigurationResponse CreateResponse(FeatureEntity featureEntity, string ringName)
        {
            var ringState = featureEntity
                .FeatureStates
                .Where(r => r.RingEntity.Name == ringName)
                .SingleOrDefault();

            var feature = new DetailsConfigurationResponse
            {
                FeatureName = featureEntity.Name,
                Enabled = ringState?.Enabled ?? false
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
                            if (parameters.ContainsKey(item.Name))
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
