using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;

namespace Esquio.EntityFrameworkCore.Store
{
    internal class StoreDbContext : DbContext
    {
        private readonly StoreOptions storeOptions;

        public StoreDbContext(DbContextOptions<StoreDbContext> options, StoreOptions storeOptions) : base(options)
        {
            this.storeOptions = storeOptions;
        }

        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<ToggleEntity> Toggles { get; set; }
        public DbSet<ParameterEntity> Parameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly, storeOptions);

            base.OnModelCreating(modelBuilder);
        }
    }
}
