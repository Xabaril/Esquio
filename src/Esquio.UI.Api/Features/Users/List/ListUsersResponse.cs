using System.Collections.Generic;

namespace Esquio.UI.Api.Features.Users.List
{
    public class ListUsersResponse
    {
        public List<ListUsersResponseDetail> UserPermissions { get; set; }
    }

    public class ListUsersResponseDetail
    {
        public string SubjectId { get; set; }

        public bool ManagementPermission { get; set; }

        public bool ReadPermission { get; set; }

        public bool WritePermission { get; set; }
    }
}
