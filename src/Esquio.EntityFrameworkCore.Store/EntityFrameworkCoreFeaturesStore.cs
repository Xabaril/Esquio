using Esquio.Abstractions;
using Esquio.EntityFrameworkCore.Store.Diagnostics;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.EntityFrameworkCore.Store
{
    internal class EntityFrameworkCoreFeaturesStore
        : IRuntimeFeatureStore
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly EsquioEntityFrameworkCoreStoreDiagnostics _diagnostics;

        public EntityFrameworkCoreFeaturesStore(StoreDbContext storeDbContext, EsquioEntityFrameworkCoreStoreDiagnostics diagnostics)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = productName ?? throw new ArgumentNullException(nameof(productName));
            _ = ringName ?? throw new ArgumentNullException(nameof(ringName));

            _diagnostics.FindFeature(featureName, productName);

            var featureEntity = await _storeDbContext
                .Features
                .Where(f => f.Name == featureName && f.ProductEntity.Name == productName)
                .Include(f => f.Toggles)
                    .ThenInclude(t => t.Parameters)
                .SingleOrDefaultAsync(cancellationToken);

            if (featureEntity != null)
            {
                _diagnostics.FeatureExist(featureName, productName);

                return ConvertToFeatureModel(featureEntity);
            }
            else
            {
                _diagnostics.FeatureNotExist(featureName, productName);

                return null;
            }
        }

        private Feature ConvertToFeatureModel(FeatureEntity featureEntity)
        {
            var feature = new Feature(featureEntity.Name);

            if (featureEntity.Enabled)
            {
                feature.Enabled();
            }
            else
            {
                feature.Disabled();
            }

            foreach (var toggleConfiguration in featureEntity.Toggles)
            {
                var toggle = new Toggle(toggleConfiguration.Type);

                toggle.AddParameters(toggleConfiguration
                    .Parameters
                    .Select(p => new Parameter(p.Name, p.Value)));

                feature.AddToggle(toggle);
            }

            return feature;
        }
    }
}
