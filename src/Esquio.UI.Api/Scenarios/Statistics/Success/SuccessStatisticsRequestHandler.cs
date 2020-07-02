using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Statistics.Success;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Statistics.Success
{
    public class SuccessStatisticsRequestHandler
        : IRequestHandler<SuccessStatisticsRequest, SuccessStatisticResponse>
    {
        private readonly StoreDbContext _store;
        private readonly ILogger<SuccessStatisticsRequestHandler> _logger;

        public SuccessStatisticsRequestHandler(StoreDbContext store, ILogger<SuccessStatisticsRequestHandler> logger)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SuccessStatisticResponse> Handle(SuccessStatisticsRequest request, CancellationToken cancellationToken)
        {
            var oneDayPast = DateTime.Now.AddDays(-1);
            var stats = new {
                SuccessCount = await _store.Metrics.Where(m => m.DateTime > oneDayPast).SumAsync(
                    m => m.Kind == "Success" ? 1 : 0),
                CountTotal = await _store.Metrics.Where(m => m.DateTime > oneDayPast).CountAsync()
            };
            int percentage = 0;
            // Sanity
            if (stats.SuccessCount >= 0 && stats.CountTotal > 0)
                percentage = (int)(stats.SuccessCount / (double)stats.CountTotal * 100.0);
            else
                return null;
            return new SuccessStatisticResponse{ 
                PercentageSuccess = percentage
            };
        }
    }
}
