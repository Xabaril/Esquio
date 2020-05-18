namespace Esquio
{
    /// <summary>
    /// The collection of Esquio constants
    /// </summary>
    public class EsquioConstants
    {
        /// <summary>
        /// Default split separator
        /// </summary>
        public static char[] DEFAULT_SPLIT_SEPARATOR = new char[] { ';' };
        /// <summary>
        /// Default Esquio name
        /// </summary>
        public const string ESQUIO = nameof(ESQUIO);
        /// <summary>
        /// Default product name.
        /// </summary>
        public const string DEFAULT_PRODUCT_NAME = "default";
        /// <summary>
        /// Default deployment name.
        /// </summary>
        public const string DEFAULT_DEPLOYMENT_NAME = "Tests";

        // - toggle value types 
        /// <summary>
        /// Semicolon type name.
        /// </summary>
        public const string SEMICOLON_LIST_PARAMETER_TYPE = "Esquio.SemicolonList";
        /// <summary>
        /// Percentage type name.
        /// </summary>
        public const string PERCENTAGE_PARAMETER_TYPE = "Esquio.Percentage";
        /// <summary>
        /// Date type name.
        /// </summary>
        public const string DATE_PARAMETER_TYPE = "Esquio.Date";
        /// <summary>
        /// String type name.
        /// </summary>
        public const string STRING_PARAMETER_TYPE = "Esquio.String";

        // - dotnet counters
        /// <summary>
        /// Feature evaluation counter name.
        /// </summary>
        public const string FEATURE_EVALUATION_PER_SECOND_COUNTER = "feature-evaluation";
        /// <summary>
        /// Feature throws per second counter name.
        /// </summary>
        public const string FEATURE_THROWS_PER_SECOND_COUNTER = "feature-evaluation-throws";
        /// <summary>
        /// Toggle evaluation per second counter name.
        /// </summary>
        public const string TOGGLE_EVALUATION_PER_SECOND_COUNTER = "toggle-activation";
        /// <summary>
        /// Toggle activation throws counter name.
        /// </summary>
        public const string TOGGLE_ACTIVATION_THROWS_PER_SECOND_COUNTER = "toggle-activation-throws";
        /// <summary>
        /// Feature not found counter name.
        /// </summary>
        public const string FEATURE_NOTFOUND_COUNTER = "feature-evaluation-notfound";


        // - diagnostic listener activities
        /// <summary>
        /// Esquio listener name
        /// </summary>
        public const string ESQUIO_LISTENER_NAME = ESQUIO;
        /// <summary>
        /// The Esquio begin feature activity name.
        /// </summary>
        public const string ESQUIO_BEGINFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationBegin";
        /// <summary>
        /// The Esquio end feature activity name.
        /// </summary>
        public const string ESQUIO_ENDFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationEnd";
        /// <summary>
        /// The Esquio throw feature activity name.
        /// </summary>
        public const string ESQUIO_THROWFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationThrow";
        /// <summary>
        /// The Esquio not found feature activity name.
        /// </summary>
        public const string ESQUIO_NOTFOUNDFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationNotFound";
        /// <summary>
        /// The Esquio begin toggle feature activity name.
        /// </summary>
        public const string ESQUIO_BEGINTOGGLE_ACTIVITY_NAME = "Esquio.ToggleExecutionBegin";
        /// <summary>
        /// The Esquio end toggle feature activity name.
        /// </summary>
        public const string ESQUIO_ENDTOGGLE_ACTIVITY_NAME = "Esquio.ToggleExecutionEnd";
    }
}


