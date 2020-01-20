using System.Collections.Generic;

namespace Esquio.UI.Api.Scenarios.Products.List
{
    public class ListProductResponse
    {
        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public List<ListProductResponseDetail> Result { get; set; }
    }

    public class ListProductResponseDetail
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
