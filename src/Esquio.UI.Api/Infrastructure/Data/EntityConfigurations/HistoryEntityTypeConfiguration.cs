using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class HistoryEntityTypeConfiguration : IEntityTypeConfiguration<Entities.HistoryEntity>
    {
        private readonly StoreOptions _storeOptions;

        public HistoryEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<Entities.HistoryEntity> builder)
        {
            builder.ToTable(_storeOptions.History);
            builder.HasKey(p => p.Id);
        }
    }
}
