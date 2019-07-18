using System;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace Esquio.Diagnostics
{
    [EventSource(Name = "Esquio")]
    public sealed class Counters
        : EventSource
    {
        public static Counters Instance = new Counters();

        private readonly ConcurrentDictionary<string, EventCounter> _featureEvaluationDynamicCounters;
        private readonly ConcurrentDictionary<string, EventCounter> _toggleEvaluationDynamicCounters;

        private Counters()
            : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            _featureEvaluationDynamicCounters = new ConcurrentDictionary<string, EventCounter>();
            _toggleEvaluationDynamicCounters = new ConcurrentDictionary<string, EventCounter>();
        }

        public void FeatureEvaluationTime(string featureName, double elapsedMilliseconds)
        {
            if (featureName != null)
            {
                var counterName = $"{featureName}-evaluation-time";

                if (!_featureEvaluationDynamicCounters.TryGetValue(counterName, out EventCounter counter))
                {
                    counter = new EventCounter(counterName, this);

                    _featureEvaluationDynamicCounters.TryAdd(counterName, counter);
                }

                WriteEvent(1, featureName, elapsedMilliseconds);

                counter?.WriteMetric((float)Math.Round(elapsedMilliseconds, 2));
            }
        }

        public void ToggleEvaluationTime(string featureName, string toggleName, double elapsedMilliseconds)
        {
            if (featureName != null && toggleName != null)
            {
                var counterName = $"{featureName}-{toggleName}-evaluation-time";

                if (!_toggleEvaluationDynamicCounters.TryGetValue(counterName, out EventCounter counter))
                {
                    counter = new EventCounter(counterName, this);

                    _toggleEvaluationDynamicCounters.TryAdd(counterName, counter);
                }

                WriteEvent(2, featureName, toggleName, elapsedMilliseconds);

                counter?.WriteMetric((float)Math.Round(elapsedMilliseconds, 2));
            }
        }
    }
}
