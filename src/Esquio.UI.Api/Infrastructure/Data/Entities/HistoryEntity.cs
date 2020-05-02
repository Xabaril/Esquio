using System;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public class HistoryEntity
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string ProductName { get; set; }
        public string FeatureName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NewValues { get; set; }
        public string OldValues { get; set; }
    }
}
