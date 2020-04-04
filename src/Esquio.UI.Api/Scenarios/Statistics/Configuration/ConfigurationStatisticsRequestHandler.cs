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
            using (var command = _store.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText =
                @$"SELECT (SELECT COUNT(id) FROM dbo.Products) as {nameof(ConfigurationStatisticsResponse.TotalProducts)}, " +
                    $"(SELECT COUNT(id) FROM dbo.Features) as {nameof(ConfigurationStatisticsResponse.TotalFeatures)}, " +
                    $"(SELECT COUNT(id) FROM dbo.Rings) AS {nameof(ConfigurationStatisticsResponse.TotalRings)}," +
                    $"(SELECT COUNT(id) FROM dbo.Toggles) AS {nameof(ConfigurationStatisticsResponse.TotalToggles)}";

                command.CommandType = CommandType.Text;

                await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken))
                {

                    if (await reader.ReadAsync(cancellationToken))
                    {
                        var response = new ConfigurationStatisticsResponse()
                        {
                            TotalProducts = reader.GetInt32(reader.GetOrdinal(nameof(ConfigurationStatisticsResponse.TotalProducts))),
                            TotalFeatures = reader.GetInt32(reader.GetOrdinal(nameof(ConfigurationStatisticsResponse.TotalFeatures))),
                            TotalRings = reader.GetInt32(reader.GetOrdinal(nameof(ConfigurationStatisticsResponse.TotalRings))),
                            TotalToggles = reader.GetInt32(reader.GetOrdinal(nameof(ConfigurationStatisticsResponse.TotalToggles)))
                        };

                        return response;
                    }

                    return null;
                }
            }
        }
    }
}
