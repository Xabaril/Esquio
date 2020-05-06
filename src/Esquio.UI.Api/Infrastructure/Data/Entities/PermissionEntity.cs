using System;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public class PermissionEntity
    {
        public int Id { get; set; }

        public string SubjectId { get; set; }

        public SubjectType Kind { get; set; }

        public ApplicationRole ApplicationRole { get; set; }
    }

    [Flags]
    public enum ApplicationRole
        : short
    {
        Reader = 1,
        Contributor = 3,
        Management = 7
    }

    public enum SubjectType
    {
        User = 1,
        Application = 2
    }
}
