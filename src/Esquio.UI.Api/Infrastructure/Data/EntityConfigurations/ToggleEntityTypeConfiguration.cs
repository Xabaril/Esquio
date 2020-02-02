using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class ToggleEntityTypeConfiguration : IEntityTypeConfiguration<Entities.ToggleEntity>
    {
        private readonly StoreOptions storeOption;

        public ToggleEntityTypeConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<Entities.ToggleEntity> builder)
        {
            builder.ToTable(storeOption.Toggles);
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
