using System.Collections.Generic;

namespace Esquio.UI.Api.Infrastructure.Settings
{
    public class SecuritySettings
    {
        public string DefaultSubjectClaimType { get; set; } = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public bool IsAzureAd { get; set; } = false;

        public List<SecuritySettingsUser> DefaultUsers { get; set; } = new List<SecuritySettingsUser>();

        public SecuritySettingsOpenId OpenId { get; set; }
    }

    public class SecuritySettingsUser
    {
        public string ApplicationRole { get; set; }

        public string SubjectId { get; set; }
    }

    public class SecuritySettingsOpenId
    {
        public string ClientId { get; set; }

        public string Audience { get; set; }

        public string Scope { get; set; }

        public string Authority { get; set; }

        public string ResponseType { get; set; }
    }
}
