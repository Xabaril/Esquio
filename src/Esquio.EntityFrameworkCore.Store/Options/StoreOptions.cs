using Microsoft.EntityFrameworkCore;
using System;

namespace Esquio.EntityFrameworkCore.Store.Options
{
    public class StoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
        public Action<IServiceProvider,DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }
        public string DefaultSchema { get; set; } = null;
        public TableConfiguration Applications { get; set; } = new TableConfiguration("Applications");
        public TableConfiguration Features { get; set; } = new TableConfiguration("Features");
        public TableConfiguration Toggles { get; set; } = new TableConfiguration("Toggles");
        public TableConfiguration Parameters { get; set; } = new TableConfiguration("Parameters");
    }
}
