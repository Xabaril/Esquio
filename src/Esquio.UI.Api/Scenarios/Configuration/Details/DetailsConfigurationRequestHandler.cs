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
            _metricsClient = metricsClient ?? throw new ArgumentNullException(nameof(metricsClient));
        }

        public async Task<DetailsConfigurationResponse> Handle(DetailsConfigurationRequest request, CancellationToken cancellationToken)
        {
            var defaultDeployment = await _storeDbContext
                .Deployments
                .Where(d => d.ByDefault && d.ProductEntity.Name == request.ProductName)
                .SingleOrDefaultAsync(cancellationToken);

            var currentDeploymentName = request.DeploymentName ?? defaultDeployment.Name;

            var featureEntity = await _storeDbContext
                .Features
                .Where(f => f.Name == request.FeatureName && f.ProductEntity.Name == request.ProductName)
                .Include(f => f.FeatureStates)
                   .ThenInclude(t => t.DeploymentEntity)
                .Include(f => f.Toggles)
                   .ThenInclude(t => t.Parameters)
                .SingleOrDefaultAsync(cancellationToken);

            DetailsConfigurationResponse response = null;
            ConfigurationRequestMetric metric = null;

            if (featureEntity != null)
            {
                metric = ConfigurationRequestMetric.FromSuccess(request.ProductName, request.FeatureName, currentDeploymentName);
                response = CreateResponse(featureEntity, currentDeploymentName, defaultDeployment.Name);
            }
            else
            {
                metric = ConfigurationRequestMetric.FromNotFound(request.ProductName, request.FeatureName, currentDeploymentName);
                Log.FeatureNotExist(_logger, request.FeatureName);
            }

            _metricsClient.Add(metric);
            return response;
        }

        private DetailsConfigurationResponse CreateResponse(FeatureEntity featureEntity, string currentDeploymentName, string defaultDeploymentName)
        {
            var status = featureEntity
                .FeatureStates
                .Where(r => r.DeploymentEntity.Name == currentDeploymentName)
                .SingleOrDefault();

            var feature = new DetailsConfigurationResponse
            {
                FeatureName = featureEntity.Name,
                Enabled = status?.Enabled ?? false
            };

            foreach (var toggleConfiguration in featureEntity.Toggles)
            {
                var type = toggleConfiguration.Type;
                var parameters = new Dictionary<string, object>();

                var groupingParameters = toggleConfiguration.Parameters
                    .GroupBy(g => g.DeploymentName);

                var defaultDeploymentParameters = groupingParameters
                    .Where(g => g.Key == defaultDeploymentName)
                    .SingleOrDefault();

                if (defaultDeploymentParameters != null
                    &&
                    defaultDeploymentParameters.Any())
                {
                    foreach (var item in defaultDeploymentParameters)
                    {
                        parameters.Add(item.Name, item.Value);
                    }
                }

                if (currentDeploymentName != defaultDeploymentName)
                {
                    var selectedDeploymentParameters = groupingParameters
                        .Where(g => g.Key == currentDeploymentName)
                        .SingleOrDefault();

                    if (selectedDeploymentParameters != null
                        &&
                        selectedDeploymentParameters.Any())
                    {
                        foreach (var item in selectedDeploymentParameters)
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
