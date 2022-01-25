using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Statistics.TopFeatures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Statistics.TopFeatures
{
    public class TopFeaturesStatisticsRequestHandler
        : IRequestHandler<TopFeaturesStatisticsRequest, TopFeaturesStatisticsResponse>
    {
        private readonly StoreDbContext _store;
        private readonly ILogger<TopFeaturesStatisticsRequestHandler> _logger;

        public TopFeaturesStatisticsRequestHandler(StoreDbContext store, ILogger<TopFeaturesStatisticsRequestHandler> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TopFeaturesStatisticsResponse> Handle(TopFeaturesStatisticsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var previousDay = DateTime.UtcNow.AddDays(-1);

                var featureDetails = await _store.Metrics
                    .Where(m => m.DateTime > previousDay)
                    .GroupBy(m => m.FeatureName)
                    .Select(m => new TopFeaturesDetailsStatisticsResponse
                    {
                        FeatureName = m.Key,
                        Requests = m.Count()
                    })
                    .OrderByDescending(m => m.Requests)
                    .Take(5)
                    .ToListAsync();

                return new TopFeaturesStatisticsResponse
                {
                    TopFeaturesDetails = featureDetails
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
