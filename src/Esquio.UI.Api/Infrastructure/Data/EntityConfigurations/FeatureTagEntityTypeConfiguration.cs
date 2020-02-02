using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class FeatureTagEntityTypeConfiguration
        :IEntityTypeConfiguration<FeatureTagEntity>
    {
        private readonly StoreOptions storeOption;

        public FeatureTagEntityTypeConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<FeatureTagEntity> builder)
        {
            builder.ToTable(storeOption.FeatureTag);
            builder.HasKey(ft => new { ft.FeatureEntityId, ft.TagEntityId });
            builder.HasOne(ft => ft.TagEntity)
                .WithMany();
        }
    }
}
