using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products.List
{
    public class ListProductRequestHandler : IRequestHandler<ListProductRequest, ListProductResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public ListProductRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<ListProductResponse> Handle(ListProductRequest request, CancellationToken cancellationToken)
        {
            var total = await _storeDbContext
                .Products
                .CountAsync(cancellationToken);

            var products = await _storeDbContext
                .Products
                .Skip(request.PageIndex * request.PageCount)
                .Take(request.PageCount)
                .ToListAsync(cancellationToken);

            return new ListProductResponse()
            {
                Count = products.Count,
                Total = total,
                PageIndex = request.PageIndex,
                Result = products.Select(p => new ListProductResponseDetail
                {
                    Name = p.Name,
                    Description = p.Description
                }).ToList()
            };
        }
    }
}
