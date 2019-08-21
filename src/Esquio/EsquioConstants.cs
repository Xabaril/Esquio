namespace Esquio
{
    public class EsquioConstants
    {
        public static char[] DEFAULT_SPLIT_SEPARATOR = new char[] { ';' };

        public const string DEFAULT_PRODUCT_NAME = "default";

        // - toggle value types 
        public const string SEMICOLON_LIST_PARAMETER_TYPE = "Esquio.SemicolonList";
        public const string PERCENTAGE_PARAMETER_TYPE = "Esquio.Percentage";
        public const string DATE_PARAMETER_TYPE = "Esquio.Date";
        public const string STRING_PARAMETER_TYPE = "Esquio.String";

        // - dotnet counters
        public const string FEATURE_EVALUATION_PER_SECOND_COUNTER = "feature-evaluation";
        public const string FEATURE_THROWS_PER_SECOND_COUNTER = "feature-evaluation-throws";
        public const string TOGGLE_EVALUATION_PER_SECOND_COUNTER = "toggle-activation";
        public const string TOGGLE_ACTIVATION_THROWS_PER_SECOND_COUNTER = "toggle-activation-throws";
        public const string FEATURE_NOTFOUND_COUNTER = "feature-evaluation-notfound";


        // - diagnostic listener activies
        public const string ESQUIO_BEGINFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationBegin";
        public const string ESQUIO_ENDFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationEnd";
        public const string ESQUIO_THROWFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationThrow";
        public const string ESQUIO_NOTFOUNDFEATURE_ACTIVITY_NAME = "Esquio.FeatureEvaluationNotFound";
        public const string ESQUIO_BEGINTOGGLE_ACTIVITY_NAME = "Esquio.ToggleExecutionBegin";
        public const string ESQUIO_ENDTOGGLE_ACTIVITY_NAME = "Esquio.ToggleExecutionEnd";
    }
}


