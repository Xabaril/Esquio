using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
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

            builder.HasAlternateKey(p => new { p.Name, p.RingName, p.ToggleEntityId });
        }
    }
}
