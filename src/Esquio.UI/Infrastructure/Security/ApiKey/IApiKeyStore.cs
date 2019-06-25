using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Esquio.UI.Infrastructure.Security.ApiKey
{
    internal interface IApiKeyStore
    {
        Task<ClaimsIdentity> ValidateApiKey(string apiKey, string authenticationScheme);
    }

    public class DefaultApiKeyStore
        :IApiKeyStore
    {
        const string DEFAULT_APIKEY_USERNAME = "APIKEY-USER";

        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DefaultApiKeyStore> _logger;

        public DefaultApiKeyStore(StoreDbContext storeDbContext, ILogger<DefaultApiKeyStore> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ClaimsIdentity> ValidateApiKey(string apiKey, string authenticationScheme)
        {
            Log.ApiKeyStoreValidating(_logger);

            var configuredKey = await _storeDbContext
                .ApiKeys
                .Where(ak => ak.Key == apiKey)
                .SingleOrDefaultAsync();

            if (configuredKey != null)
            {
                Log.ApiKeyStoreKeyExist(_logger);

                return new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,DEFAULT_APIKEY_USERNAME)
                }, authenticationScheme);
            }

            return default;
        }
    }
}
