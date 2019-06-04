using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.ApiKeys.Add
{
    public class AddAddApiKeyRequestHandler : IRequestHandler<AddApiKeyRequest, int>
    {
        private readonly StoreDbContext _storeDbContext;

        private IApiKeyGenerator ApiKeyGenerator { get; set; } = new DefaultRandomApiKeyGenerator();

        public AddAddApiKeyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }

        public async Task<int> Handle(AddApiKeyRequest request, CancellationToken cancellationToken)
        {
            var existing = await _storeDbContext
                .ApiKeys
                .Where(ak => ak.Name == request.Name)
                .SingleOrDefaultAsync();

            if (existing == null)
            {
                var key = ApiKeyGenerator
                    .CreateApiKey();

                var apiKey = new ApiKeyEntity(
                    request.Name,
                    request.Description,
                    key);

                _storeDbContext.Add(apiKey);
                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return apiKey.Id;
            }

            throw new InvalidOperationException($"A ApiKey with name {request.Name} already exist.");
        }
    }
}
