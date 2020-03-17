using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Features.List;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Flags.List
{
    public class ListFeatureRequestHandler : IRequestHandler<ListFeatureRequest, PaginatedResult<ListFeatureResponseDetail>>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListFeatureRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<PaginatedResult<ListFeatureResponseDetail>> Handle(ListFeatureRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .Features
                .Where(f => f.ProductEntity.Name == request.ProductName && !f.Archived)
                .CountAsync(cancellationToken);

            var features = await _storeDbContext
                .Features
                .Where(f => f.ProductEntity.Name == request.ProductName && !f.Archived)
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<ListFeatureResponseDetail>()
            {
                Count = features.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Items = features.Select(f => new ListFeatureResponseDetail
                {
                    Enabled = f.Enabled,
                    Description = f.Description,
                    Name = f.Name
                }).ToList()
            };
        }
    }
}
