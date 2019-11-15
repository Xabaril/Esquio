using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.ApiKeys.List
{
    public class ListApiKeyRequestHandler : IRequestHandler<ListApiKeyRequest, ListApiKeyResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListApiKeyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<ListApiKeyResponse> Handle(ListApiKeyRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .ApiKeys
                .Where(key => key.ValidTo >= DateTime.UtcNow)
                .CountAsync(cancellationToken);

            var apiKeys = await _storeDbContext
                .ApiKeys
                .Where(key => key.ValidTo >= DateTime.UtcNow)
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new ListApiKeyResponse()
            {
                Count = apiKeys.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = apiKeys.Select(ak => new ListApiKeyResponseDetail
                {
                    Name = ak.Name,
                    ValidTo = ak.ValidTo,
                }).ToList()
            };
        }
    }
}
