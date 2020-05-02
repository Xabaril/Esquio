using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.ApiKeys.List;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.ApiKeys.List
{
    public class ListApiKeyRequestHandler : IRequestHandler<ListApiKeyRequest, PaginatedResult<ListApiKeyResponseDetail>>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListApiKeyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<PaginatedResult<ListApiKeyResponseDetail>> Handle(ListApiKeyRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .ApiKeys
                .Where(key => key.ValidTo >= DateTime.UtcNow)
                .CountAsync(cancellationToken);

            var apiKeys = await _storeDbContext
                .ApiKeys
                .Where(key => key.ValidTo >= DateTime.UtcNow)
                .OrderBy(key => key.Name)
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<ListApiKeyResponseDetail>()
            {
                Count = apiKeys.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Items = apiKeys.Select(ak => new ListApiKeyResponseDetail
                {
                    Name = ak.Name,
                    ValidTo = ak.ValidTo,
                }).ToList()
            };
        }
    }
}
