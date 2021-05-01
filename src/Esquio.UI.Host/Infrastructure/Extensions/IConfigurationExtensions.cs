using Esquio.UI.Host.Infrastructure.Configuration;
using System;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        private const string ConfigurationSection = "ConfigurationProviders:AzureAppConfiguration";

        public static AzureAppConfigurationExtendedOptions GetAzureAppConfigurationOptions(this IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = configuration.GetSection(ConfigurationSection).Get<AzureAppConfigurationExtendedOptions>();
            return options ?? new AzureAppConfigurationExtendedOptions();
        }
    }
}
