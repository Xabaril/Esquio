namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }

        public string SubjectId { get; set; }

        public bool ReadPermission { get; set; }

        public bool WritePermission { get; set; }

        public bool ManagementPermission { get; set; }
    }
}
