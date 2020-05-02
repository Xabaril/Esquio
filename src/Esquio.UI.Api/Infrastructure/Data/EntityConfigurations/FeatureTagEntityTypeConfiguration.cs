using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class FeatureTagEntityTypeConfiguration
        :IEntityTypeConfiguration<FeatureTagEntity>
    {
        private readonly StoreOptions _storeOptions;

        public FeatureTagEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<FeatureTagEntity> builder)
        {
            builder.ToTable(_storeOptions.FeatureTag);

            builder.HasKey(ft => new { ft.FeatureEntityId, ft.TagEntityId });

            builder.HasOne(ft => ft.TagEntity)
                .WithMany();
        }
    }
}
