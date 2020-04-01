using System;

namespace Esquio.UI.Api.Shared.Models.Audit.List
{

    public class ListAuditResponseDetail
    {
        public DateTime CreatedAt { get; set; }

        public string Action { get; set; }

        public string ProductName { get; set; }

        public string FeatureName { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }
    }
}
