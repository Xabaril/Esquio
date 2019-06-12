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
                .CountAsync(cancellationToken);

            var features = await _storeDbContext
                .ApiKeys
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new ListApiKeyResponse()
            {
                Count = features.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = features.Select(ak => new ListApiKeyResponseDetail
                {
                    Id = ak.Id,
                    Name = ak.Name,
                    Description = ak.Description,
                    Key = ak.Key
                }).ToList()
            };
        }
    }
}
