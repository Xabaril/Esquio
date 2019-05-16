using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ToggleEntityConfiguration : IEntityTypeConfiguration<Entities.ToggleEntity>
    {
        private readonly StoreOptions storeOption;

        public ToggleEntityConfiguration(StoreOptions storeOption)
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
            builder.HasIndex(t => t.Type)
                .IsUnique();
            builder.HasMany(t => t.Parameters)
                .WithOne()
                .HasForeignKey(nameof(ParameterEntity.ToggleEntityId))
                .IsRequired();
        }
    }
}
