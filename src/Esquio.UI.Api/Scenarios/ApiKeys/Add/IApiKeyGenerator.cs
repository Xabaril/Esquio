using System;
using System.Security.Cryptography;

namespace Esquio.UI.Api.Features.ApiKeys.Add
{
    public interface IApiKeyGenerator
    {
        string CreateApiKey();
    }

    public class DefaultRandomApiKeyGenerator
        : IApiKeyGenerator
    {
        public string CreateApiKey()
        {
            var key = new byte[32];

            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(key);
            }

            return Convert.ToBase64String(key);
        }
    }
}
