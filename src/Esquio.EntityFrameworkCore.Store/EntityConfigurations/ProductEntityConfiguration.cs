using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ProductEntityConfiguration : IEntityTypeConfiguration<Entities.ProductEntity>
    {
        private readonly StoreOptions storeOption;

        public ProductEntityConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<Entities.ProductEntity> builder)
        {
            builder.ToTable(storeOption.Products);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(p => p.Name)
                .IsUnique();
            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(2000);
        }
    }
}
