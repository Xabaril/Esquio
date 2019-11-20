using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFeatureRequestHandler : IRequestHandler<ListFeatureRequest, ListFeatureResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListFeatureRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<ListFeatureResponse> Handle(ListFeatureRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .Features
                .Where(f => f.ProductEntity.Name == request.ProductName)
                .CountAsync(cancellationToken);

            var features = await _storeDbContext
                .Features
                .Where(f => f.ProductEntity.Name == request.ProductName)
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new ListFeatureResponse()
            {
                Count = features.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = features.Select(f => new ListFlagResponseDetail
                {
                    Enabled = f.Enabled,
                    Description = f.Description,
                    Name = f.Name
                }).ToList()
            };
        }
    }
}
