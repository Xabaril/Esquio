using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Entities.ProductEntity>
    {
        private readonly StoreOptions _storeOptions;

        public ProductEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<Entities.ProductEntity> builder)
        {
            builder.ToTable(_storeOptions.Products);

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
