using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.List
{
    public class ListProductRequestHandler : IRequestHandler<ListProductRequest, ListProductResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListProductRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }
        public async Task<ListProductResponse> Handle(ListProductRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .Products
                .CountAsync(cancellationToken);

            var features = await _storeDbContext
                .Products
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new ListProductResponse()
            {
                Count = features.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = features.Select(p => new ListProductResponseDetail
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name
                }).ToList()
            };
        }
    }
}
