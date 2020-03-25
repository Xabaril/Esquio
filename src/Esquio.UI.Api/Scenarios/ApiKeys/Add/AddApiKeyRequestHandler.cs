using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.ApiKeys.Add;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.ApiKeys.Add
{
    public class AddAddApiKeyRequestHandler : IRequestHandler<AddApiKeyRequest, AddApiKeyResponse>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddAddApiKeyRequestHandler> _logger;

        private IApiKeyGenerator ApiKeyGenerator { get; set; } = new DefaultRandomApiKeyGenerator();

        public AddAddApiKeyRequestHandler(StoreDbContext storeDbContext, ILogger<AddAddApiKeyRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AddApiKeyResponse> Handle(AddApiKeyRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .ApiKeys
                .Where(ak => ak.Name == request.Name)
                .SingleOrDefaultAsync();

            if (existing == null)
            {
                var key = ApiKeyGenerator.CreateApiKey();

                var apiKey = new ApiKeyEntity(
                    name: request.Name,
                    key: key,
                    validTo: request.ValidTo ?? DateTime.UtcNow.AddYears(1));

                var apiKeyPermissions = new PermissionEntity()
                {
                    SubjectId = key,
                    Kind = SubjectType.Application,
                    ApplicationRole = Enum.Parse<ApplicationRole>(request.ActAs, ignoreCase: true)
                };

                _storeDbContext
                   .Add(apiKeyPermissions);

                _storeDbContext
                    .Add(apiKey);

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return new AddApiKeyResponse()
                {
                    Name = apiKey.Name,
                    Key = key
                };
            }

            Log.ApiKeyAlreadyExist(_logger, request.Name);
            throw new InvalidOperationException($"A ApiKey with name {request.Name} already exist.");
        }
    }
}
