using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Statistics.TopFeatures;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (var command = _store.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText =
@$"SELECT TOP 5 FeatureName as {nameof(TopFeaturesDetailsStatisticsResponse.FeatureName)},count(*) as {nameof(TopFeaturesDetailsStatisticsResponse.Requests)}
FROM [dbo].[Metrics] WHERE DateTime > DATEADD(HOUR, -24,[DateTime])
GROUP BY FeatureName
ORDER BY COUNT(*) desc";

                command.CommandType = CommandType.Text;

                await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken))
                {
                    var response = new TopFeaturesStatisticsResponse();
                    var details = new List<TopFeaturesDetailsStatisticsResponse>();

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var item = new TopFeaturesDetailsStatisticsResponse()
                        {
                            FeatureName = reader.GetString(reader.GetOrdinal(nameof(TopFeaturesDetailsStatisticsResponse.FeatureName))),
                            Requests = reader.GetInt32(reader.GetOrdinal(nameof(TopFeaturesDetailsStatisticsResponse.Requests))),
                        };
                        details.Add(item);
                    }

                    response.TopFeaturesDetails = details;
                    return response;
                }
            }
        }
    }
}
