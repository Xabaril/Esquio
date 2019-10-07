using System.Security.Claims;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public const string SubjectClaimType = ClaimTypes.NameIdentifier;
        public const string IssuerClaimType = "iss";

        public static string GetSubjectId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(SubjectClaimType);
        }

        public static bool IsBearer(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(IssuerClaimType) != null;
        }
    }
}
