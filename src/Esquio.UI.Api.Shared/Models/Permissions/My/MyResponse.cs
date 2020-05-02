namespace Esquio.UI.Api.Shared.Models.Permissions.My
{
    public class MyResponse
    {
        public bool IsAuthorized { get; set; }

        public string ActAs { get; set; }

        public static MyResponse UnAuthorized()
        {
            return new MyResponse()
            {
                IsAuthorized = false
            };
        }
    }
}
