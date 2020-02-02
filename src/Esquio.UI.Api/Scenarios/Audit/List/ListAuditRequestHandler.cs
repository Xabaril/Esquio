using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Audit.List
{
    public class ListAuditRequestHandler : IRequestHandler<ListAuditRequest, ListAuditResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListAuditRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<ListAuditResponse> Handle(ListAuditRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .History
                .CountAsync(cancellationToken);

            var history = await _storeDbContext.History
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .OrderByDescending(h=>h.CreatedAt)
                .ToListAsync(cancellationToken);

            return new ListAuditResponse()
            {
                Count = history.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = history.Select(h => new ListAuditResponseDetail
                {
                    CreatedAt = h.CreatedAt,
                    Action = h.Action,
                    FeatureName = h.FeatureName,
                    ProductName = h.ProductName,
                    OldValues = h.OldValues,
                    NewValues = h.NewValues
                }).ToList()
            };
        }
    }
}
