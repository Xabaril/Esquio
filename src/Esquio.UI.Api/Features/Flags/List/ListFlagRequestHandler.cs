using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Features.Flags.List;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class ListFlagRequestHandler : IRequestHandler<ListFlagRequest, ListFlagResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListFlagRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
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
