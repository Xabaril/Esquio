using Esquio.Abstractions;
using Esquio.Database.Store.Diagnostics;
using Esquio.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Database.Store
{
    internal class EsquioDatabaseStore
        : IRuntimeFeatureStore
    {
        private readonly string _connectionString;
        private readonly EsquioDatabaseStoreDiagnostics _diagnostics;

        public EsquioDatabaseStore(string connectionString,EsquioDatabaseStoreDiagnostics diagnostics)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public async Task<Feature> FindFeatureAsync(string featureName, string productName, string ringName, CancellationToken cancellationToken = default)
        {
            _ = featureName ?? throw new ArgumentNullException(nameof(featureName));
            _ = productName ?? throw new ArgumentNullException(nameof(productName));
            _ = ringName ?? throw new ArgumentNullException(nameof(ringName));

            _diagnostics.FindFeature(featureName, productName, ringName);

            //TODO: sql to get feature

            throw new NotImplementedException("sd");

            //_diagnostics.RequestFailed(response.RequestMessage.RequestUri, response.StatusCode);
            throw new InvalidOperationException("Distributed store response is not success status code.");
        }
    }
}
