namespace Esquio.UI.Api.Shared.Models
{
    public class Constants
    {
        public static class Constraints
        {
            public const string NamesRegularExpression = "^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$";

            public const string HexColorRegularExpression = "^#{1}[0-9A-F]{6}$";
        }
        public static class Pagination
        {
            public const int PageIndex = 0;
            public const int PageCount = 10;
        }
    }
}
