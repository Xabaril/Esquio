using Esquio.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebAppCustomToggle.Toggles
{
    public class MyCustomToggle : IToggle
    {
        private readonly ILogger<MyCustomToggle> _logger;

        public MyCustomToggle(ILogger<MyCustomToggle> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public Task<bool> IsActiveAsync(string featureName, string applicationName = null)
        {
            _logger.LogInformation($"Feature {featureName} for Application {applicationName} IsActiveAsync()");

            return Task.FromResult(true);
        }
    }
}
