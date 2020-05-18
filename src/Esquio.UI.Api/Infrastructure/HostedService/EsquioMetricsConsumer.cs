using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Infrastructure.HostedService
{
    public class EsquioMetricsConsumer
        : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly EsquioMetricsClient _metrics;
        private readonly ILogger<EsquioMetricsConsumer> _logger;

        public EsquioMetricsConsumer(IServiceScopeFactory serviceScopeFactory, EsquioMetricsClient metrics, ILogger<EsquioMetricsConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Esquio metrics consumer is starting");

            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                _logger.LogInformation("The Esquio metrics consumer is saving all metrics on database.");

                try
                {
                    var buffer = new List<ConfigurationRequestMetric>();

                    while (!_metrics.IsEmpty())
                    {
                        buffer.Add(_metrics.Take());
                    }

                    if (buffer.Any())
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var store = scope.ServiceProvider
                                .GetRequiredService<StoreDbContext>();

                            store.Metrics.AddRange(buffer.Select(item => new MetricEntity()
                            {
                                ProductName = item.ProductName,
                                DeploymentName = item.DeploymentName,
                                FeatureName = item.FeatureName,
                                DateTime = item.RequestedAt,
                                Kind = item.Kind
                            }).ToList());

                            await store.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Esquio metrics consumer is throwing when checking metrics to consume.");
                }

                await Task.Delay(10 * 1000, stoppingToken);

            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}
