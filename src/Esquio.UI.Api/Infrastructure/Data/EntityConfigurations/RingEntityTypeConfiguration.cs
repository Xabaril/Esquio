using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    public class RingEntityTypeConfiguration
        : IEntityTypeConfiguration<Entities.RingEntity>
    {
        private readonly StoreOptions _storeOptions;

        public RingEntityTypeConfiguration(StoreOptions storeOption)
        {
            this._storeOptions = storeOption ?? throw new ArgumentNullException(nameof(storeOption));
        }

        public void Configure(EntityTypeBuilder<RingEntity> builder)
        {
            builder.ToTable(_storeOptions.Rings);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ByDefault)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(r => r.ProductEntity)
                .WithMany(p => p.Rings)
                .HasForeignKey(r => r.ProductEntityId);
        }
    }
}
