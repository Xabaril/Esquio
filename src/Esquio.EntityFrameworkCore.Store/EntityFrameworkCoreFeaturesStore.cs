using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    internal class EntityFrameworkCoreFeaturesStore : IFeatureStore
    {
        private readonly ILogger<EntityFrameworkCoreFeaturesStore> _logger;
        private readonly StoreDbContext _dbContext;

        public EntityFrameworkCoreFeaturesStore(ILogger<EntityFrameworkCoreFeaturesStore> logger, StoreDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public bool IsReadOnly => false;

        public async Task<bool> AddFeatureAsync(string featureName, string applicationName, bool enabled = false)
        {
            var application = new ProductEntity
            {
                Name = applicationName,
                Description = applicationName
            };

            var feature = new FeatureEntity
            {
                Name = featureName,
                CreatedOn = DateTime.UtcNow,
                Enabled = enabled,
                Description = featureName
            };

            application.Features.Add(feature);

            await _dbContext.Products.AddAsync(application);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddFeatureAsync(string applicationName, Feature feature)
        {
            var application = new ProductEntity
            {
                Name = applicationName,
                Description = applicationName
            };

            application.Features.Add(feature.To());

            await _dbContext.Products.AddAsync(application);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddToggleAsync<TToggle>(string featureName, string applicationName, IDictionary<string, object> parameterValues) where TToggle : IToggle
        {
            var feature = await FindFeatureEntityAsync(featureName, applicationName);

            if (feature == null)
            {
                return false;
            }

            var toggle = new ToggleEntity
            {
                FeatureId = feature.Id,
                Parameters = parameterValues
                    .Select(p =>
                        new ParameterEntity
                        {
                            Name = p.Key,
                            Value = p.Value.ToString()
                        })
                    .ToList()
            };

            await _dbContext.AddAsync(toggle);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string applicationName)
        {
            FeatureEntity feature = await FindFeatureEntityAsync(featureName, applicationName);

            if (feature != null)
            {
                return feature.To();
            }

            return null;
        }

        public async Task<IEnumerable<string>> FindTogglesTypesAsync(string featureName, string applicationName)
        {
            var toggles = await _dbContext
                .Products
                .Where(a => a.Name == applicationName)
                .Join(_dbContext.Features,
                    a => a.Id,
                    f => f.ApplicationId,
                    (a, f) => f)
                .Where(f => f.Name == featureName)
                .Join(_dbContext.Toggles,
                    f => f.Id,
                    t => t.FeatureId,
                    (f,t) => t)
                .Select(t => t.Type)
                .ToListAsync();

            return toggles.AsEnumerable();
        }

        public async Task<object> GetToggleParameterValueAsync<TToggle>(string featureName, string applicationName, string parameterName) where TToggle : IToggle
        {
            var parameter = await _dbContext
                .Products
                .Where(a => a.Name == applicationName)
                .Join(_dbContext.Features,
                    a => a.Id,
                    f => f.ApplicationId,
                    (a, f) => f)
                .Where(f => f.Name == featureName)
                .Join(_dbContext.Toggles,
                    f => f.Id,
                    t => t.FeatureId,
                    (f, t) => t)
                .Where(t => t.Type == typeof(TToggle).FullName)
                .Join(_dbContext.Parameters,
                    t => t.Id,
                    p => p.ToggleId,
                    (t, p) => p)
                .SingleOrDefaultAsync(p => p.Name == parameterName);

            if (parameter != null)
            {
                return parameter.Value;
            }

            return null;
        }

        private async Task<FeatureEntity> FindFeatureEntityAsync(string featureName, string applicationName)
        {
            return await _dbContext
                .Products
                .Where(a => a.Name == applicationName)
                .Join(_dbContext.Features,
                    a => a.Id,
                    f => f.ApplicationId,
                    (a, f) => f)
                .SingleOrDefaultAsync(f => f.Name == featureName);
        }
    }
}
