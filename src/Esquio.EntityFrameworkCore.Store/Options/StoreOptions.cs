using Microsoft.EntityFrameworkCore;
using System;

namespace Esquio.EntityFrameworkCore.Store.Options
{
    public class StoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
        public Action<IServiceProvider,DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }
        public string DefaultSchema { get; set; } = null;
        public TableConfiguration Products { get; set; } = new TableConfiguration("Products");
        public TableConfiguration Features { get; set; } = new TableConfiguration("Features");
        public TableConfiguration Toggles { get; set; } = new TableConfiguration("Toggles");
        public TableConfiguration Parameters { get; set; } = new TableConfiguration("Parameters");
        public TableConfiguration Tags { get; set; } = new TableConfiguration("Tags");
        public TableConfiguration FeatureTag { get; set; } = new TableConfiguration("FeatureTags");
        public TableConfiguration ApiKeys { get; set; } = new TableConfiguration("ApiKeys");
    }
}
