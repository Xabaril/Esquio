using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql;
namespace Esquio.UI.Api.Infrastructure.Data.DbContexts
{
    public class MySqlContext : StoreDbContext
    {
        public MySqlContext(DbContextOptions dbContextOptions, IOptions<StoreOptions> storeOptions) : base(dbContextOptions, storeOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //override and fix incompatible configurations on my sql 

            modelBuilder.Entity<PermissionEntity>().Property(p => p.ApplicationRole)
              .HasConversion<string>()
              .IsRequired();

            modelBuilder.Entity<PermissionEntity>().Property(p => p.Kind)
                .HasConversion<string>()
                .IsRequired();
        }

    }
}
