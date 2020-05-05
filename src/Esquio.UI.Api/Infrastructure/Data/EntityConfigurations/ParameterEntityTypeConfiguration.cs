using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class ParameterEntityTypeConfiguration : IEntityTypeConfiguration<ParameterEntity>
    {
        private readonly StoreOptions _storeOptions;

        public ParameterEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<ParameterEntity> builder)
        {
            builder.ToTable(_storeOptions.Parameters);

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(p => p.RingName)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(p => p.Value)
              .IsRequired()
              .HasMaxLength(4000);

            builder.HasAlternateKey(p => new { p.Name, p.RingName, p.ToggleEntityId });
        }
    }
}
