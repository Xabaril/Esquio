namespace Esquio.UI.Api
{
    public static class ApiConstants
    {
        public static class Pagination
        {
            public const int PageIndex = 1;
            public const int PageSize = 10;
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
