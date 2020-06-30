using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Statistics.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Statistics.Configuration
{
    public class ConfigurationStatisticsRequestHandler
        : IRequestHandler<ConfigurationStatisticsRequest, ConfigurationStatisticsResponse>
    {
        private readonly StoreDbContext _store;
        private readonly ILogger<ConfigurationStatisticsRequestHandler> _logger;

        public ConfigurationStatisticsRequestHandler(StoreDbContext store, ILogger<ConfigurationStatisticsRequestHandler> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ConfigurationStatisticsResponse> Handle(ConfigurationStatisticsRequest request, CancellationToken cancellationToken)
        {
            return new ConfigurationStatisticsResponse {
                TotalProducts = await _store.Products.CountAsync(),
                TotalFeatures = await _store.Features.CountAsync(),
                TotalDeployments = await _store.Deployments.CountAsync(),
                TotalToggles = await _store.Toggles.CountAsync()
            };            
        }
    }
}
