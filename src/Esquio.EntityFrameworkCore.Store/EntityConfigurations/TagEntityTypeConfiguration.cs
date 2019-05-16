using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class TagEntityTypeConfiguration
        : IEntityTypeConfiguration<TagEntity>
    {
        private readonly StoreOptions storeOption;

        public TagEntityTypeConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.ToTable(storeOption.Tags);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
