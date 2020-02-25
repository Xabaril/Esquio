using Esquio.UI.Api.Infrastructure.Data.Entities;
using System;

namespace Esquio.UI.Api.Scenarios.Users.My
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

        public static MyResponse With(ApplicationRole role)
        {
            return new MyResponse()
            {
                ActAs = Enum.GetName(typeof(ApplicationRole), role),
                IsAuthorized = true
            };
        }
    }
}
