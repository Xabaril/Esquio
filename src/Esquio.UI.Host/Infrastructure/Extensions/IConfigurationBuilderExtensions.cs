using Azure.Identity;
using Esquio.UI.Host.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace Esquio.UI.Host.Infrastructure.Extensions
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder UseAzureAppConfiguration(this IConfigurationBuilder builder)
        {
            Action<AzureAppConfigurationOptions> setupConfig = AzureAppConfiguration.UseConnectionString switch
            {
                true => options => options.Connect(AzureAppConfiguration.ConnectionString),
                false => options => options.Connect(new Uri(AzureAppConfiguration.ManagedIdentityEndpoint),
                                                    new ManagedIdentityCredential(AzureAppConfiguration.ClientId))
            };

            builder.AddAzureAppConfiguration(options =>
            {
                setupConfig(options);

                if (AzureAppConfiguration.UseCacheExpiration)
                {
                    options.ConfigureRefresh(config =>
                    {
                        config.SetCacheExpiration(TimeSpan.FromSeconds(AzureAppConfiguration.CacheExpiration));
                    });
                }

                if (AzureAppConfiguration.UseLabel)
                {
                    options.Select(KeyFilter.Any, AzureAppConfiguration.Label);
                };

            });

            return builder;
        }
    }
}
