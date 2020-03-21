using Esquio.UI.Api.Shared.Infrastructure.Data.Entities;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }

        public string SubjectId { get; set; }

        public SubjectType Kind { get; set; }

        public ApplicationRole ApplicationRole { get; set; }
    }

    public enum SubjectType
    {
        User = 1,
        Application = 2
    }
}
