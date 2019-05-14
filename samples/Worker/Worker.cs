using Esquio.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceScopeFactory scopeFactory,ILogger<Worker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var featureService = scope.ServiceProvider
                        .GetRequiredService<IFeatureService>();

                    await featureService.Do("ComputeMatch",
                        active: () =>
                        {
                            _logger.LogInformation("Worker running with ComputeMatch Feature Active at: {time}", DateTimeOffset.Now);
                        },
                        notActive: () =>
                        {
                            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                        });


                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
