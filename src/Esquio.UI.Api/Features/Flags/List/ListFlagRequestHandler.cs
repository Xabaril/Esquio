using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.List
{
    public class ListFlagRequestHandler : IRequestHandler<ListFlagRequest, ListFlagResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListFlagRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<ListFlagResponse> Handle(ListFlagRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .Features
                .Where(f => f.ProductEntityId == request.ProductId)
                .CountAsync(cancellationToken);

            var features = await _storeDbContext
                .Features
                .Where(f => f.ProductEntityId == request.ProductId)
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new ListFlagResponse()
            {
                Count = features.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = features.Select(f => new ListFlagResponseDetail
                {
                    Id = f.Id,
                    Enabled = f.Enabled,
                    Name = f.Name
                }).ToList()
            };
        }
    }
}
