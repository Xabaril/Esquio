using System;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Threading;

namespace Esquio.Diagnostics
{
    /// <summary>
    /// Esquio EventSource class ETW and performance counters.
    /// https://devblogs.microsoft.com/dotnet/introducing-diagnostics-improvements-in-net-core-3-0/ 
    /// https://github.com/dotnet/diagnostics
    /// https://github.com/dotnet/diagnostics/blob/master/documentation/dotnet-counters-instructions.md
    /// https://blogs.msdn.microsoft.com/vancem/2015/09/14/exploring-eventsource-activity-correlation-and-causation-features/
    /// </summary>
    internal sealed class EsquioEventSource
        : EventSource
    {
        public static readonly EsquioEventSource Log = new EsquioEventSource();

        long _perSecondFeatureEvaluations;
        long _perSecondFeatureThrows;
        long _perSecondFeatureNotFound;
        long _perSecondToggleEvaluations;
        long _perSecondActivationThrows;

        IncrementingPollingCounter _featuresPerSecondCounter;
        IncrementingPollingCounter _featureThrowPerSecondCounter;
        IncrementingPollingCounter _featureNotFoundPerSecondCounter;
        IncrementingPollingCounter _togglePerSecondCounter;
        IncrementingPollingCounter _toggleActivationThrowPerSecondCounter;

        //-dinamic counters

        private readonly ConcurrentDictionary<string, EventCounter> _featureDynamicCounters;

        public EsquioEventSource()
            : base("Esquio", EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            _featuresPerSecondCounter = new IncrementingPollingCounter(EsquioConstants.FEATURE_EVALUATION_PER_SECOND_COUNTER, this, () => _perSecondFeatureEvaluations)
            {
                DisplayName = "Feature Evaluations",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            _featureThrowPerSecondCounter = new IncrementingPollingCounter(EsquioConstants.FEATURE_THROWS_PER_SECOND_COUNTER, this, () => _perSecondFeatureThrows)
            {
                DisplayName = "Feature Evaluations Throws",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            _featureNotFoundPerSecondCounter = new IncrementingPollingCounter(EsquioConstants.FEATURE_NOTFOUND_COUNTER, this, () => _perSecondFeatureNotFound)
            {
                DisplayName = "Feature Evaluations NotFound",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            _togglePerSecondCounter = new IncrementingPollingCounter(EsquioConstants.TOGGLE_EVALUATION_PER_SECOND_COUNTER, this, () => _perSecondToggleEvaluations)
            {
                DisplayName = "Toggle Executions",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            _toggleActivationThrowPerSecondCounter = new IncrementingPollingCounter(EsquioConstants.TOGGLE_ACTIVATION_THROWS_PER_SECOND_COUNTER, this, () => _perSecondActivationThrows)
            {
                DisplayName = "Toggle Executions Throws",
                DisplayRateTimeScale = new TimeSpan(0, 0, 1)
            };

            _featureDynamicCounters = new ConcurrentDictionary<string, EventCounter>();
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
        public void FeatureEvaluationThrow(string featureName, string productName, string ringName, string error)
        {
            Interlocked.Increment(ref _perSecondFeatureThrows);
            WriteEvent(3, featureName, productName, ringName, error);
        }

        [Event(4, Level = EventLevel.Informational)]
        public void FeatureEvaluated(string featureName, string productName, string ringName, long elapsedMilliseconds)
        {
            Interlocked.Increment(ref _perSecondFeatureEvaluations);

            if (featureName != null)
            {
                var counterName = $"{featureName.Trim().ToLower()}-evaluation-time";

                if (!_featureDynamicCounters.TryGetValue(counterName, out EventCounter counter))
                {
                    counter = new EventCounter(counterName, this)
                    {
                        DisplayName = $"Feature [{featureName.Trim().ToLower()}] Evaluation Time / ms"
                    };

                    _featureDynamicCounters.TryAdd(counterName, counter);
                }

                counter?.WriteMetric(elapsedMilliseconds);
            }

            WriteEvent(4, featureName, productName, ringName, elapsedMilliseconds);
        }

        [Event(5, Level = EventLevel.Error)]
        public void FeatureEvaluationNotFound(string featureName, string productName, string ringName)
        {
            Interlocked.Increment(ref _perSecondFeatureNotFound);
            WriteEvent(5, featureName, productName, ringName);
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
        public void ToggleEvaluated(string featureName, string productName, string ringName, string toggle, long elapsedMilliseconds)
        {
            Interlocked.Increment(ref _perSecondToggleEvaluations);
            WriteEvent(12, featureName, productName, ringName, toggle, elapsedMilliseconds);
        }

        [Event(20, Level = EventLevel.Informational)]
        public void ToggleActivationStart()
        {
            WriteEvent(20);
        }

        [Event(21, Level = EventLevel.Informational)]
        public void ToggleActivationStop(string toggleTypeName)
        {
            WriteEvent(21, toggleTypeName);
        }

        [Event(22, Level = EventLevel.Error)]
        public void ToggleActivationCantCreateInstance(string toggleTypeName)
        {
            Interlocked.Increment(ref _perSecondActivationThrows);
            WriteEvent(22, toggleTypeName);
        }
    }
}
