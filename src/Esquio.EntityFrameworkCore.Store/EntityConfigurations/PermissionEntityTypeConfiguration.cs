using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esquio.EntityFrameworkCore.Store.EntityConfigurations
{
    internal class PermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<PermissionEntity>
    {
        private readonly StoreOptions storeOption;

        public PermissionEntityTypeConfiguration(StoreOptions storeOption)
        {
            this.storeOption = storeOption;
        }

        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable(storeOption.Permissions.Name,storeOption.Permissions.Schema);
            builder.Property(p => p.SubjectId)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(p => p.SubjectId)
                .IsUnique();
            builder.Property(p => p.ReadPermission)
                .HasDefaultValue(false);
            builder.Property(p => p.WritePermission)
                .HasDefaultValue(false);
            builder.Property(p => p.ManagementPermission)
                .HasDefaultValue(false);
        }
    }
}
