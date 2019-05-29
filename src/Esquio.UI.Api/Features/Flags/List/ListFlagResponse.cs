using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFlagResponse
    {
        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public List<ListFlagResponseDetail> Result { get; set; }
    }

    public class ListFlagResponseDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }
    }
}
