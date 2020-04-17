using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Statistics.Plot;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (var command = _store.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText =
@$"
DECLARE @END DATETIME2  = GETDATE();
DECLARE @START DATETIME2 = DATEADD(HOUR, -24, @END);
WITH CTE_Dates AS
(
    SELECT @START AS cte_date
    UNION ALL
    SELECT DATEADD(SECOND, 30, cte_date) AS cte_date
    FROM CTE_Dates
    WHERE cte_date <= @END
)
SELECT CTE_Dates.cte_date as {nameof(PlotPointStatisticsResponse.Date)}, COUNT(Metrics.Id) AS {nameof(PlotPointStatisticsResponse.Value)}
FROM CTE_Dates
LEFT JOIN Metrics
ON Metrics.[DateTime] BETWEEN cte_date AND DATEADD(SECOND, 30, cte_date)
GROUP BY cte_date
OPTION (MAXRECURSION 10000)
";
                command.CommandType = CommandType.Text;
                await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken))
                {
                    var response = new PlotStatisticsResponse();
                    var points = new List<PlotPointStatisticsResponse>(capacity: 1200); //points over 12 hours each 30 seconds aprox

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var item = new PlotPointStatisticsResponse()
                        {
                            Date = reader.GetDateTime(reader.GetOrdinal(nameof(PlotPointStatisticsResponse.Date))),
                            Value = reader.GetInt32(reader.GetOrdinal(nameof(PlotPointStatisticsResponse.Value))),
                        };

                        points.Add(item);
                    }

                    response.Points = points;
                    return response;
                }
            }
        }
    }
}
