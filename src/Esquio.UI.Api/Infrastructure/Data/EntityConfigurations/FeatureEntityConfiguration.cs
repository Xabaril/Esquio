using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class FeatureEntityConfiguration : IEntityTypeConfiguration<Entities.FeatureEntity>
    {
        private readonly StoreOptions _storeOptions;

        public FeatureEntityConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<Entities.FeatureEntity> builder)
        {
            builder.ToTable(_storeOptions.Features);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(p => p.Enabled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.Archived)
               .IsRequired()
               .HasDefaultValue(false);

            builder.HasOne(f => f.ProductEntity)
                .WithMany(p => p.Features)
                .HasForeignKey(f => f.ProductEntityId);
        }
    }
}
