using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models.ApiKeys.Details;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.ApiKeys.Details
{
    public class DetailsApiKeyRequestHandler : IRequestHandler<DetailsApiKeyRequest, DetailsApiKeyResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public DetailsApiKeyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<DetailsApiKeyResponse> Handle(DetailsApiKeyRequest request, CancellationToken cancellationToken)
        {
            var apiKey = await (from ak in _storeDbContext.ApiKeys
                                join p in _storeDbContext.Permissions on ak.Key equals p.SubjectId
                                where ak.Name == request.Name
                                select new { ak.Name, ak.ValidTo, p.ApplicationRole }).SingleOrDefaultAsync();

            if (apiKey != null)
            {
                return new DetailsApiKeyResponse()
                {
                    Name = apiKey.Name,
                    ActAs = apiKey.ApplicationRole.ToString(),
                    ValidTo = apiKey.ValidTo,
                };
            }

            return null;
        }
    }
}
