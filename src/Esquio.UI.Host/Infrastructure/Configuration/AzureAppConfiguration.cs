namespace Esquio.UI.Host.Infrastructure.Configuration
{
    public class AzureAppConfiguration
    {
        public static bool Enabled
        {
            get
            {
                if (EnvironmentVariable.HasValue(AzureAppConfigurationKeys.Enabled))
                {
                    if (bool.TryParse(EnvironmentVariable.GetValue(AzureAppConfigurationKeys.Enabled), out bool enabled))
                    {
                        return enabled;
                    }

                    return false;
                }

                return false;
            }
        }

        public static bool UseConnectionString =>
            EnvironmentVariable.HasValue(AzureAppConfigurationKeys.ConnectionString);

        public static bool UseLabel =>
            EnvironmentVariable.HasValue(AzureAppConfigurationKeys.Label);

        public static bool UseCacheExpiration =>
            EnvironmentVariable.HasValue(AzureAppConfigurationKeys.CacheExpiration)
            && double.TryParse(EnvironmentVariable.GetValue(AzureAppConfigurationKeys.CacheExpiration), out var _);

        public static double CacheExpiration => double.Parse(EnvironmentVariable.GetValue(AzureAppConfigurationKeys.CacheExpiration));

        public static string ClientId => EnvironmentVariable.GetValue(AzureAppConfigurationKeys.ClientId);

        public static string ConnectionString =>
            EnvironmentVariable.GetValue(AzureAppConfigurationKeys.ConnectionString);

        public static string ManagedIdentityEndpoint =>
            EnvironmentVariable.GetValue(AzureAppConfigurationKeys.ManagedIdentityEndpoint);

        public static string Label =>
            EnvironmentVariable.GetValue(AzureAppConfigurationKeys.Label);
    }
}
