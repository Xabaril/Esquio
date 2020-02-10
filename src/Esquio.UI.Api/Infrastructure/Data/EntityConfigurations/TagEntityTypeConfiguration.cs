using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class TagEntityTypeConfiguration
        : IEntityTypeConfiguration<TagEntity>
    {
        private readonly StoreOptions _storeOptions;

        public TagEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.ToTable(_storeOptions.Tags);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
