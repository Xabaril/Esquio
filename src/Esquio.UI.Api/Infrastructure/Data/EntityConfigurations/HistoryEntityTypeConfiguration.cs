using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class HistoryEntityTypeConfiguration : IEntityTypeConfiguration<Entities.HistoryEntity>
    {
        private readonly StoreOptions storeOption;

        public HistoryEntityTypeConfiguration(StoreOptions storeOption)
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
