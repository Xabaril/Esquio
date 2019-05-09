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
            builder.ToTable(storeOption.Applications);
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.HasMany(x => x.Features).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
        }
    }
}
