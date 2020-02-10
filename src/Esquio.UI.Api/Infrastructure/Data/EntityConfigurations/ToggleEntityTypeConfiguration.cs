using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class ToggleEntityTypeConfiguration : IEntityTypeConfiguration<Entities.ToggleEntity>
    {
        private readonly StoreOptions _storeOptions;

        public ToggleEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<Entities.ToggleEntity> builder)
        {
            builder.ToTable(_storeOptions.Toggles);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type)
               .IsRequired()
               .HasMaxLength(200);

            builder.HasAlternateKey(t => new { t.Type, t.FeatureEntityId })
                .HasName("IX_ToggeFeature");

            builder.HasMany(t => t.Parameters)
                .WithOne(t => t.ToggleEntity)
                .HasForeignKey(t => t.ToggleEntityId)
                .IsRequired();
        }
    }
}
