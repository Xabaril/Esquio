using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Threading;

namespace Esquio.Diagnostics
{
    /// <summary>
    /// https://blogs.msdn.microsoft.com/vancem/2015/09/14/exploring-eventsource-activity-correlation-and-causation-features/
    /// </summary>
    internal sealed class EsquioEventSource
        : EventSource
    {
        public static readonly EsquioEventSource Log = new EsquioEventSource();

        //TODO: change (_featuresPerSecondCounter,_featureThrowPerSecondCounter,_togglePerSecondCounter,_toggleActvationThrowPerSecondCounter) and use IncrementingPollingCounter on preview8

        long _totalFeatureEvaluations;
        long _totalFeatureThrows;
        long _totalToggleEvaluations;
        long _totalActivationThrows;

        EventCounter _featuresPerSecondCounter;
        EventCounter _featureThrowPerSecondCounter;
        EventCounter _togglePerSecondCounter;
        EventCounter _toggleActvationThrowPerSecondCounter;

        //TODO: change (_featuresNotFoundCounter ) and use IncrementEventCounter on preview8

        long _totalFeatureNotFound;
        EventCounter _featuresNotFoundCounter;

        //-dinamic counters

        private readonly ConcurrentDictionary<string, EventCounter> _featureDynamicCounters;
        private readonly ConcurrentDictionary<string, EventCounter> _toggleDynamicCounters;

        public EsquioEventSource()
            : base("Esquio", EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            _featuresPerSecondCounter = new EventCounter(EsquioConstants.FEATURE_EVALUATION_PER_SECOND_COUNTER, this);
            _featureThrowPerSecondCounter = new EventCounter(EsquioConstants.FEATURE_THROWS_PER_SECOND_COUNTER, this);
            _togglePerSecondCounter = new EventCounter(EsquioConstants.TOGGLE_EVALUATION_PER_SECOND_COUNTER, this);
            _toggleActvationThrowPerSecondCounter = new EventCounter(EsquioConstants.TOGGLE_ACTIVATION_THROWS_PER_SECOND_COUNTER, this);

            _featuresNotFoundCounter = new EventCounter(EsquioConstants.FEATURE_NOTFOUND_COUNTER, this);

            _featureDynamicCounters = new ConcurrentDictionary<string, EventCounter>();
            _toggleDynamicCounters = new ConcurrentDictionary<string, EventCounter>();
        }


        [Event(1, Level = EventLevel.Informational)]
        public void FeatureEvaluationStart()
        {
            WriteEvent(1);
        }

        [Event(2, Level = EventLevel.Informational)]
        public void FeatureEvaluationStop()
        {
            WriteEvent(2);
        }

        [Event(3, Level = EventLevel.Error)]
        public void FeatureEvaluationThrow(string featureName, string productName, string error)
        {
            productName = productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME;

            Interlocked.Increment(ref _totalFeatureThrows);
            WriteEvent(3, featureName, productName, error);
        }

        [Event(4, Level = EventLevel.Informational)]
        public void FeatureEvaluated(string featureName, string productName, long elapsedMilliseconds)
        {
            productName = productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME;

            Interlocked.Increment(ref _totalFeatureEvaluations);

            if (featureName != null)
            {
                var counterName = $"{featureName.Trim().ToLower()}-evaluation-time";

                if (!_featureDynamicCounters.TryGetValue(counterName, out EventCounter counter))
                {
                    counter = new EventCounter(counterName, this);

                    _featureDynamicCounters.TryAdd(counterName, counter);
                }

                counter?.WriteMetric(elapsedMilliseconds);
            }

            WriteEvent(4, featureName, productName, elapsedMilliseconds);
        }

        [Event(5, Level = EventLevel.Error)]
        public void FeatureEvaluationNotFound(string featureName, string productName)
        {
            productName = productName ?? EsquioConstants.DEFAULT_PRODUCT_NAME;

            Interlocked.Increment(ref _totalFeatureNotFound);
            WriteEvent(5, featureName, productName);
        }


        [Event(10, Level = EventLevel.Informational)]
        public void ToggleEvaluationStart()
        {
            WriteEvent(10);
        }

        [Event(11, Level = EventLevel.Informational)]
        public void ToggleEvaluationStop()
        {
            WriteEvent(11);
        }

        [Event(12, Level = EventLevel.Informational)]
        public void ToggleEvaluated(string featureName, string productName, string toggle, long elapsedMilliseconds)
        {
            Interlocked.Increment(ref _totalToggleEvaluations);

            if (featureName != null && toggle != null)
            {
                var counterName = $"{featureName.Trim().ToLower()}-{toggle.Trim().ToLower()}-evaluation-time";

                if (!_toggleDynamicCounters.TryGetValue(counterName, out EventCounter counter))
                {
                    counter = new EventCounter(counterName, this);

                    _toggleDynamicCounters.TryAdd(counterName, counter);
                }

                counter?.WriteMetric(elapsedMilliseconds);
            }

            WriteEvent(12, featureName, productName, toggle, elapsedMilliseconds);
        }

        [Event(20, Level = EventLevel.Informational)]
        public void ToggleActivationStart()
        {
            WriteEvent(20);
        }

        [Event(21, Level = EventLevel.Informational)]
        public void ToggleActivationStop()
        {
            WriteEvent(21);
        }

        [Event(22, Level = EventLevel.Error)]
        public void ToggleActivationCantCreateInstance(string toggleTypeName)
        {
            Interlocked.Increment(ref _totalActivationThrows);
            WriteEvent(22, toggleTypeName);
        }
    }
}
