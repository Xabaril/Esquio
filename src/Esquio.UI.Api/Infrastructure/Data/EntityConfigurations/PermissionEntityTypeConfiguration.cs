﻿using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Api.Shared.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Esquio.UI.Api.Infrastructure.Data.EntityConfigurations
{
    internal class PermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<PermissionEntity>
    {
        private readonly StoreOptions _storeOptions;

        public PermissionEntityTypeConfiguration(StoreOptions storeOptions)
        {
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable(_storeOptions.Permissions);

            builder.Property(p => p.SubjectId)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(p => p.SubjectId)
                .IsUnique();

            builder.Property(p => p.ApplicationRole)
                .HasDefaultValue(ApplicationRole.Reader)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.Kind)
                .HasDefaultValue(SubjectType.User)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
