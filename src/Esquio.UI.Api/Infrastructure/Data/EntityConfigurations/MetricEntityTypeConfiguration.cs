using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{

    internal class MetricEntityTypeConfiguration : IEntityTypeConfiguration<Entities.MetricEntity>
    {
        private readonly StoreOptions _storeOptions;

        public MetricEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<Entities.MetricEntity> builder)
        {
            builder.ToTable(_storeOptions.Metrics);
            builder.HasIndex(m => m.DateTime);
            builder.HasKey(p => p.Id);
        }
    }
}
