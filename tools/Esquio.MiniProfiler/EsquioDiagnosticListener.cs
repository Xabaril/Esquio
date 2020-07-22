using Esquio;
using Esquio.Diagnostics;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MiniProfiler.Esquio
{
    internal class EsquioDiagnosticListener
        : IMiniProfilerDiagnosticListener
    {
        private readonly Dictionary<Guid, CustomTiming> _featureEvaluations
            = new Dictionary<Guid, CustomTiming>();

        private readonly Dictionary<Guid, CustomTiming> _toggleExecutions
            = new Dictionary<Guid, CustomTiming>();

        public string ListenerName => EsquioConstants.ESQUIO_LISTENER_NAME;

        public void OnCompleted() { }

        public void OnError(Exception error) => Trace.WriteLine(error);


        public void OnNext(KeyValuePair<string, object> entry)
        {
            if (entry.Key == EsquioConstants.ESQUIO_BEGINFEATURE_ACTIVITY_NAME)
            {
                var data = entry.Value as FeatureEvaluatingEventData;

                _featureEvaluations.Add(
                    data.CorrelationId,
                    global::StackExchange.Profiling.MiniProfiler.Current.CustomTiming(ListenerName, $"Esquio Feature Evaluation {data.Feature} on product {data.Product} with deployment {data.Deployment}", "Feature Evaluation"));
            }
            else if (entry.Key == EsquioConstants.ESQUIO_ENDFEATURE_ACTIVITY_NAME)
            {
                var data = entry.Value as FeatureEvaluatedEventData;

                if (_featureEvaluations.TryRemove(data.CorrelationId, out CustomTiming timing))
                {
                    timing.StackTraceSnippet = "Feature Evaluated";
                    timing?.Stop();
                }
            }
            else if (entry.Key == EsquioConstants.ESQUIO_NOTFOUNDFEATURE_ACTIVITY_NAME)
            {
                var data = entry.Value as FeatureNotFoundEventData;

                if (_featureEvaluations.TryRemove(data.CorrelationId, out CustomTiming timing))
                {
                    timing.Errored = true;
                    timing.StackTraceSnippet = "Feature NotFound";
                    timing?.Stop();
                }
            }
            else if (entry.Key == EsquioConstants.ESQUIO_THROWFEATURE_ACTIVITY_NAME)
            {
                var data = entry.Value as FeatureThrowEventData;

                if (_featureEvaluations.TryRemove(data.CorrelationId, out CustomTiming timing))
                {
                    if (timing != null)
                    {
                        timing.Errored = true;
                        timing.StackTraceSnippet = "Toggle Execution Failure";
                        timing.Stop();
                    }
                }
            }
            else if (entry.Key == EsquioConstants.ESQUIO_BEGINTOGGLE_ACTIVITY_NAME)
            {
                var data = entry.Value as ToggleEvaluatingEventData;

                _toggleExecutions.Add(
                    data.CorrelationId,
                    global::StackExchange.Profiling.MiniProfiler.Current.CustomTiming(ListenerName, $"Esquio Toggle Execution {data.Feature}:{data.ToggleType} on product {data.Product} with deployment {data.Deployment}", "Toggle Execution"));
            }
            else if (entry.Key == EsquioConstants.ESQUIO_ENDTOGGLE_ACTIVITY_NAME)
            {
                var data = entry.Value as ToggleEvaluatedEventData;

                if (_toggleExecutions.TryRemove(data.CorrelationId, out CustomTiming timing))
                {
                    if (timing != null)
                    {
                        timing.StackTraceSnippet = "Toggle Execution success";
                        timing.Stop();
                    }
                }
            }
        }
    }
}
