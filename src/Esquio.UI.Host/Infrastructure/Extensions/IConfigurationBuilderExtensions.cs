using Azure.Identity;
using Esquio.UI.Host.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace Esquio.UI.Host.Infrastructure.Extensions
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAzureAppConfiguration(this IConfigurationBuilder builder, AzureAppConfigurationExtendedOptions extendedOptions)
        {
            Action<AzureAppConfigurationOptions> setupConfig = extendedOptions.UseConnectionString switch
            {
                true => options => options.Connect(extendedOptions.ConnectionString),
                false => options => options.Connect(extendedOptions.Endpoint,
                                                    new ManagedIdentityCredential(extendedOptions.ClientId))
            };

            builder.AddAzureAppConfiguration(options =>
            {
                setupConfig(options);

                if (extendedOptions.UseCacheExpiration)
                {
                    options.ConfigureRefresh(config =>
                    {
                        config.SetCacheExpiration(TimeSpan.FromSeconds(extendedOptions.CacheExpiration));
                    });
                }

                foreach (var label in extendedOptions.Labels)
                {
                    options.Select(KeyFilter.Any, label);
                }

            });

            return builder;
        }
    }
}
