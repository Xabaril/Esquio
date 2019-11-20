using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFeatureResponse
    {
        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public List<ListFlagResponseDetail> Result { get; set; }
    }

    public class ListFlagResponseDetail
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }
}
