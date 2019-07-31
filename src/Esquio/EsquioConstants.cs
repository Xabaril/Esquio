namespace Esquio
{
    public class EsquioConstants
    {
        public static char[] DEFAULT_SPLIT_SEPARATOR = new char[] { ';' };

        public const string DEFAULT_PRODUCT_NAME = "default";

        public const string SEMICOLON_LIST_PARAMETER_TYPE = "Esquio.SemicolonList";
        public const string PERCENTAGE_PARAMETER_TYPE = "Esquio.Percentage";
        public const string DATE_PARAMETER_TYPE = "Esquio.Date";
        public const string STRING_PARAMETER_TYPE = "Esquio.String";

        public const string FEATURE_EVALUATION_PER_SECOND_COUNTER = "feature-evaluations-per-second";
        public const string FEATURE_THROWS_PER_SECOND_COUNTER = "features-throws-per-second";
        public const string TOGGLE_EVALUATION_PER_SECOND_COUNTER = "toggle-evaluations-per-second";
        public const string TOGGLE_ACTIVATION_THROWS_PER_SECOND_COUNTER = "toggle-activation-throws-per-second";
        public const string FEATURE_NOTFOUND_COUNTER = "features-notfound-total";
    }
}


