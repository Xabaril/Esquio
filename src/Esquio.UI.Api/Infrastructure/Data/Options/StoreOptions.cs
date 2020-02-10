using Microsoft.EntityFrameworkCore;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.Options
{
    /// <summary>
    /// Provide programatically configuration for <see cref="StoreDbContext"/>.
    /// </summary>
    public class StoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// Get or set a new action for add new configuration for <see cref="DbContextOptionsBuilder"/>
        /// </summary>
        public Action<IServiceProvider,DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        /// <summary>
        /// Get or set default schema for store configuration tables.
        /// </summary>
        public string DefaultSchema { get; set; } = "dbo";

        /// <summary>
        /// Get or set default table configuration for Products.
        /// </summary>
        public TableConfiguration Products { get; set; } = new TableConfiguration("Products");

        /// <summary>
        /// Get or set default table configuration for Features.
        /// </summary>
        public TableConfiguration Features { get; set; } = new TableConfiguration("Features");

        /// <summary>
        /// Get or set default table configuration for Toggles.
        /// </summary>
        public TableConfiguration Toggles { get; set; } = new TableConfiguration("Toggles");

        /// <summary>
        /// Get or set default table configuration for Parameters.
        /// </summary>
        public TableConfiguration Parameters { get; set; } = new TableConfiguration("Parameters");

        /// <summary>
        /// Get or set default table configuration for Tags.
        /// </summary>
        public TableConfiguration Tags { get; set; } = new TableConfiguration("Tags");

        /// <summary>
        /// Get or set default table configuration for FeatureTags.
        /// </summary>
        public TableConfiguration FeatureTag { get; set; } = new TableConfiguration("FeatureTags");

        /// <summary>
        /// Get or set default table configuration for ApiKeys.
        /// </summary>
        public TableConfiguration ApiKeys { get; set; } = new TableConfiguration("ApiKeys");

        /// <summary>
        /// Get or set default table configuration for History
        /// </summary>
        public TableConfiguration History { get; set; } = new TableConfiguration("History");

        /// <summary>
        /// Get or set default table configuration for Permissions
        /// </summary>
        public TableConfiguration Permissions { get; set; } = new TableConfiguration("Permissions");

        /// <summary>
        /// Get or set default table configuration for Rings.
        /// </summary>
        public TableConfiguration Rings { get; set; } = new TableConfiguration("Rings");

    }
}
