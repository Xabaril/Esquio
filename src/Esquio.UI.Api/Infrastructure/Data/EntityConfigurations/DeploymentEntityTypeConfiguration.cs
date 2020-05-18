using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    public class DeploymentEntityTypeConfiguration
        : IEntityTypeConfiguration<Entities.DeploymentEntity>
    {
        private readonly StoreOptions _storeOptions;

        public DeploymentEntityTypeConfiguration(StoreOptions storeOption)
        {
            this._storeOptions = storeOption ?? throw new ArgumentNullException(nameof(storeOption));
        }

        public void Configure(EntityTypeBuilder<DeploymentEntity> builder)
        {
            builder.ToTable(_storeOptions.Deployments);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ByDefault)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(r => r.ProductEntity)
                .WithMany(p => p.Deployments)
                .HasForeignKey(r => r.ProductEntityId);
        }
    }
}
