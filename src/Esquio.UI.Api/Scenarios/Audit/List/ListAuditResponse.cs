using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Audit.List
{
    public class ListAuditResponse
    {
        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public List<ListAuditResponseDetail> Result { get; set; }
    }

    public class ListAuditResponseDetail
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }
    }
}
