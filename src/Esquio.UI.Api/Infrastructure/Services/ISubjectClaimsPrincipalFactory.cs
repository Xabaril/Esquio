using System.Security.Claims;

namespace Esquio.UI.Api.Infrastructure.Services
{
    public interface ISubjectClaimsPrincipalFactory
    {
        string GetSubject(ClaimsPrincipal principal);
    }
}
