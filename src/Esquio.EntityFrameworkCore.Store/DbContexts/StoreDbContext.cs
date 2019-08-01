using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.EntityFrameworkCore.Store.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    public class StoreDbContext : DbContext
    {
        private readonly StoreOptions storeOptions;

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : this(options, new StoreOptions()) { }

        public StoreDbContext(DbContextOptions<StoreDbContext> options, StoreOptions storeOptions) : base(options)
        {
            this.storeOptions = storeOptions;
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<ToggleEntity> Toggles { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<ParameterEntity> Parameters { get; set; }
        public DbSet<FeatureTagEntity> FeatureTagEntities { get; set; }
        public DbSet<ApiKeyEntity> ApiKeys { get; set; }
        public DbSet<HistoryEntity> History { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly, storeOptions);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var entries = OnBeforeSaveChanges();
            base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSaveChanges(entries);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = OnBeforeSaveChanges();
            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            OnAfterSaveChanges(entries);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private List<HistoryEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var historyEntries = new List<HistoryEntry>();

            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                var historyEntry = new HistoryEntry(entry);

                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                foreach (var property in entry.Properties)
                {
                    var propertyName = property.Metadata.Name;

                    if (property.Metadata.IsForeignKey())
                    {
                        continue;
                    }

                    if (property.IsTemporary)
                    {
                        historyEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        historyEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            historyEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                historyEntry.OldValues[propertyName] = property.OriginalValue;
                                historyEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                        case EntityState.Added:
                            historyEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        default:
                            break;
                    }
                }

                historyEntries.Add(historyEntry);
            }

            return historyEntries;
        }

        private void OnAfterSaveChanges(List<HistoryEntry> historyEntries)
        {
            foreach (var historyEntry in historyEntries)
            {
                foreach (var property in historyEntry.TemporaryProperties)
                {
                    if (property.Metadata.IsPrimaryKey())
                    {
                        historyEntry.KeyValues[property.Metadata.Name] = property.CurrentValue;
                    }
                    else
                    {
                        historyEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                    }
                }

                History.Add(historyEntry.To(ChangeTracker));
            }
        }

        private class HistoryEntry
        {
            public HistoryEntry(EntityEntry entry)
            {
                Entry = entry;
            }

            public int MyProperty { get; set; }
            public EntityEntry Entry { get; set; }
            public List<PropertyEntry> TemporaryProperties { get; set; } = new List<PropertyEntry>();
            public Dictionary<string, object> KeyValues { get; set; } = new Dictionary<string, object>();
            public Dictionary<string, object> OldValues { get; set; } = new Dictionary<string, object>();
            public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string, object>();

            public HistoryEntity To(ChangeTracker changeTracker)
            {
                return new HistoryEntity
                {
                    FeatureId = GetFeatureId(changeTracker),
                    CreatedAt = DateTime.UtcNow,
                    KeyValues = KeyValues.Count > 0 ? JsonConvert.SerializeObject(KeyValues) : null,
                    NewValues = NewValues.Count > 0 ? JsonConvert.SerializeObject(NewValues) : null,
                    OldValues = NewValues.Count > 0 ? JsonConvert.SerializeObject(OldValues) : null
                };
            }

            private int GetFeatureId(ChangeTracker changeTracker)
            {
                if (Entry.Entity is FeatureEntity feature)
                {
                    return feature.Id;
                }
                else if (Entry.Entity is ToggleEntity toggle)
                {
                    return toggle.FeatureEntityId;
                }
                else if (Entry.Entity is ParameterEntity parameter)
                {
                    return changeTracker.Entries<ToggleEntity>()
                        .Single(t => t.Entity.Id == parameter.ToggleEntityId)
                        .Entity
                        .FeatureEntityId;
                }

                throw new Exception("Invalid entity type.");
            }
        }
    }
}
