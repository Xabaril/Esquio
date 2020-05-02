using System.Collections.Generic;

namespace Esquio.UI.Api.Shared.Models
{
    public class PaginatedResult<TDetail>
    {
        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public List<TDetail> Items { get; set; }
    }
}
