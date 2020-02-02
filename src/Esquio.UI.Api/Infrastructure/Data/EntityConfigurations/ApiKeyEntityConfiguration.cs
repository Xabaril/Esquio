using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class ApiKeyEntityConfiguration : IEntityTypeConfiguration<Entities.ApiKeyEntity>
    {
        private readonly StoreOptions storeOption;

        public ApiKeyEntityConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<Entities.ApiKeyEntity> builder)
        {
            builder.ToTable(storeOption.ApiKeys);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(p => p.Name)
                .IsUnique();
            builder.Property(p => p.ValidTo)
                .IsRequired();
            builder.Property(p => p.Key)
                .IsRequired()
                .HasMaxLength(50);
            builder.HasIndex(p => p.Key)
                .IsUnique();
        }
    }
}
