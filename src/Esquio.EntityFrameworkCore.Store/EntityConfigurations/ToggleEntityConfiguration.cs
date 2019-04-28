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
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Type).HasMaxLength(1000).IsRequired();
            builder.HasMany(x => x.Parameters).WithOne(x => x.Toggle).HasForeignKey(x => x.ToggleId);
        }
    }
}
