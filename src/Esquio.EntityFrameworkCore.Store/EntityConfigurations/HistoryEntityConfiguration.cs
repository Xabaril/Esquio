using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class HistoryEntityConfiguration : IEntityTypeConfiguration<Entities.HistoryEntity>
    {
        private readonly StoreOptions storeOption;

        public HistoryEntityConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<Entities.HistoryEntity> builder)
        {
            builder.ToTable(storeOption.History);
            builder.HasKey(p => p.Id);
        }
    }
}
