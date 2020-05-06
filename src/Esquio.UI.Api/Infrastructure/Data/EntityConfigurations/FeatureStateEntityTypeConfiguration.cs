using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    class FeatureStateEntityTypeConfiguration
        : IEntityTypeConfiguration<FeatureStateEntity>
    {
        private readonly StoreOptions _storeOptions;

        public FeatureStateEntityTypeConfiguration(StoreOptions storeOptions)
        {
            _storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<FeatureStateEntity> builder)
        {
            builder.ToTable(_storeOptions.FeatureState);

            builder.HasKey(fs => new { fs.FeatureEntityId, fs.DeploymentEntityId });

            builder.HasOne(fs => fs.FeatureEntity)
                .WithMany(f=>f.FeatureStates)
                .HasForeignKey(fs => fs.FeatureEntityId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(fs => fs.DeploymentEntity)
                .WithMany()
                .HasForeignKey(fs => fs.DeploymentEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
