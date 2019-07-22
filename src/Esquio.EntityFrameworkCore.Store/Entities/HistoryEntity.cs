using System;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class HistoryEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string KeyValues { get; set; }
        public string NewValues { get; set; }
        public string OldValues { get; set; }
    }
}
