namespace Esquio.UI.Api
{
    static class ApiConstants
    {
        public static string ApiVersionHeaderName = "X-Api-Version";
        public static class Constraints
        {
            public const string NamesRegularExpression = "^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$";
        }
        public static class Pagination
        {
            public const int PageIndex = 0;
            public const int PageCount = 10;
        }

        public static class Messages
        {
            public const string ModelStateValidation = "Please refer to the errors property for additional details.";
        }

        public static class ContentTypes
        {
            public const string ProblemJson = "application/problem+json";
            public const string ProblemXml = "application/problem+xml";
        }
    }
}
