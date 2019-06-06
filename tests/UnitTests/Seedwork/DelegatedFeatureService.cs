using Esquio.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    internal class DelegatedFeatureService
        : IFeatureService
    {
        private readonly Func<string, string, bool> _delegatedFunction;
        public DelegatedFeatureService(Func<string, string, bool> delegatedFunction)
        {
            _delegatedFunction = delegatedFunction ?? throw new ArgumentNullException(nameof(delegatedFunction));
        }
        public Task<bool> IsEnabledAsync(string featureName, string productName = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_delegatedFunction(featureName, productName));
        }
    }
}
