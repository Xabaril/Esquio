using Esquio.UI.Api;

namespace System.Security.Claims
{
    static class ClaimsPrincipalExtensions
    {
        public static string GetSubjectId(this ClaimsPrincipal principal)
        {
            return principal
                .FindFirstValue(ApiConstants.SubjectNameIdentifier);
        }
    }
}
