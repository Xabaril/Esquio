using System;

namespace Esquio.Diagnostics
{
    /// <summary>
    /// Event data class used on Esquio <see cref="System.Diagnostics.DiagnosticListener"/> listener.
    /// </summary>
    public sealed class FeatureEvaluatingEventData
    {
        /// <summary>
        /// Feature evaluation correlation id
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// The feature name
        /// </summary>
        public string Feature { get; private set; }

        /// <summary>
        /// The product name
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="FeatureEvaluatingEventData"/>
        /// </summary>
        /// <param name="correlationId">The feature evaluation correlation id.</param>
        /// <param name="feature">The feature name.</param>
        /// <param name="product">The product name.</param>
        public FeatureEvaluatingEventData(Guid correlationId, string feature, string productName)
        {
            CorrelationId = correlationId;
            Feature = feature;
            Product = productName;
        }
    }

    /// <summary>
    /// Event data class used on Esquio <see cref="System.Diagnostics.DiagnosticListener"/> listener.
    /// </summary>
    public sealed class FeatureNotFoundEventData
    {
        /// <summary>
        /// Feature evaluation correlation id
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// The feature name
        /// </summary>
        public string Feature { get; private set; }

        /// <summary>
        /// The product name
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="FeatureNotFoundEventData"/>
        /// </summary>
        /// <param name="correlationId">The feature evaluation correlation id.</param>
        /// <param name="feature">The feature name.</param>
        /// <param name="product">The product name.</param>
        public FeatureNotFoundEventData(Guid correlationId, string feature, string productName)
        {
            CorrelationId = correlationId;
            Feature = feature;
            Product = productName;
        }
    }

    /// <summary>
    /// Event data class used on Esquio <see cref="System.Diagnostics.DiagnosticListener"/> listener.
    /// </summary>
    public sealed class FeatureThrowEventData
    {
        /// <summary>
        /// Feature evaluation correlation id.
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// The featue name.
        /// </summary>
        public string Feature { get; private set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// Feature evaluation exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="FeatureThrowEventData"/>
        /// </summary>
        /// <param name="correlationId">The feature evaluation correlation id.</param>
        /// <param name="feature">The feature name.</param>
        /// <param name="product">The product name.</param>
        /// <param name="exception">Evaluation exception.</param>
        public FeatureThrowEventData(Guid correlationId, string feature, string product, Exception exception)
        {
            CorrelationId = correlationId;
            Feature = feature;
            Product = product;
            Exception = exception;
        }
    }

    /// <summary>
    /// Event data class used on Esquio <see cref="System.Diagnostics.DiagnosticListener"/> listener.
    /// </summary>
    public sealed class FeatureEvaluatedEventData
    {
        /// <summary>
        /// Feature evaluation correlation id.
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// The featue name.
        /// </summary>
        public string Feature { get; private set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// If feature evaluation result is enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Feature evaluation elapsed.
        /// </summary>
        public long Elapsed { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="FeatureEvaluatedEventData"/>
        /// </summary>
        /// <param name="correlationId">The feature evaluation correlation id.</param>
        /// <param name="feature">The feature name.</param>
        /// <param name="product">The product name.</param>
        /// <param name="enabled">Evaluation result.</param>
        /// <param name="elapsed">Evaluation elapsed time.</param>
        public FeatureEvaluatedEventData(Guid correlationId, string feature, string product, bool enabled, long elapsed)
        {
            CorrelationId = correlationId;
            Feature = feature;
            Product = product;
            Enabled = enabled;
            Elapsed = elapsed;
        }
    }

    /// <summary>
    /// Event data class used on Esquio <see cref="System.Diagnostics.DiagnosticListener"/> listener.
    /// </summary>
    public sealed class ToggleEvaluatingEventData
    {
        /// <summary>
        /// Toggle evaluation correlation id.
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// The feature name.
        /// </summary>
        public string Feature { get; private set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// The toggle type executed.
        /// </summary>
        public string ToggleType { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="FeatureEvaluatingEventData"/>
        /// </summary>
        /// <param name="correlationId">The feature evaluation correlation id.</param>
        /// <param name="feature">The feature name.</param>
        /// <param name="product">The product name.</param>
        /// <param name="toggleType">The toggle type executed.</param>
        public ToggleEvaluatingEventData(Guid correlationId, string feature, string productName, string toggleType)
        {
            CorrelationId = correlationId;
            Feature = feature;
            Product = productName;
            ToggleType = toggleType;
        }
    }

    /// <summary>
    /// Event data class used on Esquio <see cref="System.Diagnostics.DiagnosticListener"/> listener.
    /// </summary>
    public sealed class ToggleEvaluatedEventData
    {
        /// <summary>
        /// Toggle evaluation correlation id.
        /// </summary>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// The feature name.
        /// </summary>
        public string Feature { get; private set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public string Product { get; private set; }

        /// <summary>
        /// The toggle type executed.
        /// </summary>
        public string ToggleType { get; set; }

        /// <summary>
        /// The toggle activation state.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="FeatureEvaluatingEventData"/>
        /// </summary>
        /// <param name="correlationId">The feature evaluation correlation id.</param>
        /// <param name="feature">The feature name.</param>
        /// <param name="product">The product name.</param>
        /// <param name="toggleType">The toggle type executed.</param>
        /// <param name="active"`>The activation toggle state.</param>
        public ToggleEvaluatedEventData(Guid correlationId, string feature, string productName, string toggleType, bool active)
        {
            CorrelationId = correlationId;
            Feature = feature;
            Product = productName;
            ToggleType = toggleType;
            Active = active;
        }
    }
}
