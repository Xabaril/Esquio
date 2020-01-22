using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    public class RingEntityTypeConfiguration
        : IEntityTypeConfiguration<Entities.RingEntity>
    {
        private readonly StoreOptions storeOption;

        public RingEntityTypeConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<RingEntity> builder)
        {
            builder.ToTable(storeOption.Rings);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ByDefault)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(r => r.Product)
                .WithMany(p => p.Rings)
                .HasForeignKey(r => r.ProductEntityId);
        }
    }
}
