namespace Esquio.UI.Api.Features.Users.Details
{
    public class DetailsUsersResponse
    {
        public string SubjectId { get; set; }

        public bool ManagementPermission { get; set; }

        public bool ReadPermission { get; set; }

        public bool WritePermission { get; set; }
    }
}
