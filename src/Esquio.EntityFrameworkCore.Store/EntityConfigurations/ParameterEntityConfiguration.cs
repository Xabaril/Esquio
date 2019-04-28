using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class ParameterEntityConfiguration : IEntityTypeConfiguration<ParameterEntity>
    {
        private readonly StoreOptions storeOptions;

        public ParameterEntityConfiguration(StoreOptions storeOptions)
        {
            this.storeOptions = storeOptions;
        }

        public void Configure(EntityTypeBuilder<ParameterEntity> builder)
        {
            builder.ToTable(storeOptions.Parameters);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Value).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        }
    }
}
