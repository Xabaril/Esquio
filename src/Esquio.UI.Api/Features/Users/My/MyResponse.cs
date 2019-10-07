namespace Esquio.UI.Api.Features.Users.My
{
    public class MyResponse
    {
        public bool IsAuthorized { get; set; }

        public bool ReadPermission { get; set; }

        public bool WritePermission { get; set; }

        public bool ManagementPermission { get; set; }

        public static MyResponse UnAuthorized()
        {
            return new MyResponse()
            {
                ReadPermission = false,
                WritePermission = false,
                ManagementPermission = false,
                IsAuthorized = false
            };
        }

        public static MyResponse With(bool readPermission, bool writePermission, bool managementPermission)
        {
            return new MyResponse()
            {
                ReadPermission = readPermission,
                WritePermission = writePermission,
                ManagementPermission = managementPermission,
                IsAuthorized = true
            };
        }
    }
}
