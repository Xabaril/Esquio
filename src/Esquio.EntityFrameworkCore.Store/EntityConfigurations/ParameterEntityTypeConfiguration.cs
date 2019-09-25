using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ParameterEntityTypeConfiguration : IEntityTypeConfiguration<ParameterEntity>
    {
        private readonly StoreOptions storeOptions;

        public ParameterEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this.storeOptions = storeOptions;
        }

        public void Configure(EntityTypeBuilder<ParameterEntity> builder)
        {
            builder.ToTable(storeOptions.Parameters);

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(p => p.Value)
              .IsRequired()
              .HasMaxLength(4000);

            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
