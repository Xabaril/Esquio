using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Infrastructure.Security.ApiKey
{
    internal interface IApiKeyStore
    {
        Task<ClaimsIdentity> ValidateApiKey(string apiKey, string authenticationScheme);
    }

    internal class DefaultApiKeyStore
        : IApiKeyStore
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<DefaultApiKeyStore> _logger;

        public DefaultApiKeyStore(StoreDbContext StoreDbContext, ILogger<DefaultApiKeyStore> logger)
        {
            _storeDbContext= StoreDbContext ?? throw new ArgumentNullException(nameof(StoreDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ClaimsIdentity> ValidateApiKey(string apiKey, string authenticationScheme)
        {
            Log.ApiKeyStoreValidating(_logger);

            var configuredKey = await _storeDbContext
                .ApiKeys
                .Where(ak => ak.Key == apiKey)
                .SingleOrDefaultAsync();

            if (configuredKey != null && configuredKey.ValidTo >= DateTime.UtcNow)
            {
                Log.ApiKeyStoreKeyExist(_logger);

                return new ClaimsIdentity(new Claim[]
                {
                    new Claim(ApiConstants.SubjectNameIdentifier, configuredKey.Key)
                }, authenticationScheme);
            }

            return default;
        }
    }
}
