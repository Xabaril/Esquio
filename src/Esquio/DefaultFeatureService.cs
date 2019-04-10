using Esquio.Abstractions;
using System;
using System.Threading.Tasks;

namespace Esquio
{
    public sealed class DefaultFeatureService
        : IFeatureService
    {
        private readonly IFeatureStore _featureStore;

        public DefaultFeatureService(IFeatureStore store)
        {
            _featureStore = store ?? throw new ArgumentNullException(nameof(store));
        }

        public Task<bool> IsEnabledAsync(string application, string feature)
        {
            return Task.FromResult(false);
        }
    }
}
