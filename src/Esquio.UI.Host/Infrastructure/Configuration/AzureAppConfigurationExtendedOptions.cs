using System;
using System.Collections.Generic;

namespace Esquio.UI.Host.Infrastructure.Configuration
{
    public class AzureAppConfigurationExtendedOptions
    {
        /// <summary>
        /// Specify if Azure App Configuration Provider should be enabled
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// If defined it will use User Assigned Identity instead of System Assigned
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Specify the labels to load the key-value set associated
        /// </summary>
        public HashSet<string> Labels { get; set; } = new HashSet<string>();

        /// <summary>
        /// The connection string for connecting to the Service. If not defined, <see cref="Endpoint"/> must be defined to use Managed Identities
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Azure App Configuration Endpoint
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// Cache expiration time in seconds before the values are refreshed. Must be greater than 1 second.
        /// </summary>
        public double CacheExpiration { get; set; }

        internal bool UseCacheExpiration => CacheExpiration != default;
        internal bool UseConnectionString => !string.IsNullOrEmpty(ConnectionString);
    }
}
