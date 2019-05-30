using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;

namespace Esquio.EntityFrameworkCore.Store
{
    public class StoreDbContext : DbContext
    {
        private readonly StoreOptions storeOptions;

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : this(options, new StoreOptions()) { }

        public StoreDbContext(DbContextOptions<StoreDbContext> options, StoreOptions storeOptions) : base(options)
        {
            this.storeOptions = storeOptions;
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<ToggleEntity> Toggles { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<ParameterEntity> Parameters { get; set; }
        public DbSet<FeatureTagEntity> FeatureTagEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly, storeOptions);

            base.OnModelCreating(modelBuilder);
        }
    }
}
