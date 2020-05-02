using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.Statistics.Success;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
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
            using (var command = _store.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText =
@$"SELECT (SUM(CASE WHEN Kind=N'Success' THEN 1 ELSE 0 END)*100)/COUNT(id) as {nameof(SuccessStatisticResponse.PercentageSuccess)} 
FROM [dbo].[Metrics] WHERE DateTime > DATEADD(HOUR, -24,[DateTime])";

                command.CommandType = CommandType.Text;

                await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        var percentageSuccess = 0;

                        if (reader.GetValue(reader.GetOrdinal(nameof(SuccessStatisticResponse.PercentageSuccess))) != DBNull.Value)
                        {
                            percentageSuccess = reader.GetInt32(reader.GetOrdinal(nameof(SuccessStatisticResponse.PercentageSuccess)));
                        }

                        var response = new SuccessStatisticResponse()
                        {
                            PercentageSuccess = percentageSuccess
                        };

                        return response;
                    }
                }
            }

            return null;
        }
    }
}
