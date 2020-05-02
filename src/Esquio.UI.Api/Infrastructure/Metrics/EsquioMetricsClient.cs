using System.Collections.Concurrent;

namespace Esquio.UI.Api.Infrastructure.Metrics
{
    public class EsquioMetricsClient
    {
        BlockingCollection<ConfigurationRequestMetric> _blockingCollection;

        public EsquioMetricsClient()
        {
            _blockingCollection = new BlockingCollection<ConfigurationRequestMetric>();
        }

        public bool Add(ConfigurationRequestMetric metric)
        {
            if (metric != null)
            {
                return _blockingCollection.TryAdd(metric);
            }

            return false;
        }

        public bool IsCompleted()
        {
            return _blockingCollection.IsCompleted;
        }

        public bool IsEmpty()
        {
            return _blockingCollection.Count == 0;
        }

        public ConfigurationRequestMetric Take()
        {
            return _blockingCollection.Take();
        }
    }
}
