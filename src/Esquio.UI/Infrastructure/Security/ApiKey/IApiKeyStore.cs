using System.Security.Claims;

namespace Esquio.UI.Infrastructure.Security.ApiKey
{
    internal interface IApiKeyStore
    {
        ClaimsIdentity ValidateApiKey(string apiKey, string authenticationScheme);
    }
}
