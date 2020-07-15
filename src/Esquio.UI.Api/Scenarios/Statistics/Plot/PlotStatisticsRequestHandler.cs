using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Statistics.Plot;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Statistics.Plot
{
    public class PlotStatisticsRequestHandler
        : IRequestHandler<PlotStatisticsRequest, PlotStatisticsResponse>
    {

        private readonly StoreDbContext _store;
        private readonly ILogger<PlotStatisticsRequestHandler> _logger;

        public PlotStatisticsRequestHandler(StoreDbContext store, ILogger<PlotStatisticsRequestHandler> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PlotStatisticsResponse> Handle(PlotStatisticsRequest request, CancellationToken cancellationToken)
        {
            var oneDayPast = DateTime.Now.AddDays(-1);

            var oneDayMetrics = await _store.Metrics
                .Where(m => m.DateTime > oneDayPast)
                .ToListAsync();

            var groupsByTime = oneDayMetrics
                .Select(metric =>
                {
                    metric.DateTime = new DateTime(
                        metric.DateTime.Year,
                        metric.DateTime.Month,
                        metric.DateTime.Day,
                        metric.DateTime.Hour,
                        metric.DateTime.Minute,
                        metric.DateTime.Second < 30 ? 0 : 30);

                    return metric;
                }).GroupBy(m => m.DateTime).ToDictionary(m => m.Key, m => m.ToList());

            var startDate = new DateTime(oneDayPast.Year, oneDayPast.Month, oneDayPast.Day, oneDayPast.Hour, oneDayPast.Minute, 0);
            var now = DateTime.Now;

            var points = new List<PlotPointStatisticsResponse>();

            while(startDate < now )
            {
                var value = 0;

                if (groupsByTime.ContainsKey(startDate))
                {
                    value = groupsByTime[startDate].Count();
                }
                
                points.Add(new PlotPointStatisticsResponse()
                {
                    Date = startDate,
                    Value = value
                });

                startDate = startDate.AddSeconds(30);
            }

            return new PlotStatisticsResponse()
            {
                Points = points
            };
        }
    }
}
