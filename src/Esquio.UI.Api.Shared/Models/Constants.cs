using System;
using System.Collections.Generic;
using System.Text;

namespace Esquio.UI.Api.Shared.Models
{
    public class Constants
    {
        public static class Constraints
        {
            public const string NamesRegularExpression = "^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$";
        }
        public static class Pagination
        {
            public const int PageIndex = 0;
            public const int PageCount = 10;
        }
    }
}
