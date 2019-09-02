using System;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace Esquio.Diagnostics
{
    /// <summary>
    /// We need a lot of work here!, at this moment new API for counters (https://github.com/dotnet/corefx/issues/36129) 
    /// ( PollingCounter, IncrementCounter) 
    /// is not on .netstandard 2.0 or .net standard 2.1
    /// https://github.com/dotnet/standard/pull/1308 
    /// This code is used at this moment only to evaluate counters counters strategy!
    /// </summary>
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

        [Event(1,Level = EventLevel.Informational)]
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

        [Event(2, Level = EventLevel.Informational)]
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
