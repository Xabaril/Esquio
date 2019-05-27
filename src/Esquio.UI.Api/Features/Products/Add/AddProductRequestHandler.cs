using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Products.Add
{
    public class AddProductRequestHandler : IRequestHandler<AddProductRequest>
    {
        private readonly StoreDbContext dbContext;

        public AddProductRequestHandler(StoreDbContext dbContext)
        {
            Ensure.Argument.NotNull(dbContext, nameof(dbContext));
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            var product = new ProductEntity(request.Name, request.Description);
            await dbContext.AddAsync(product);
            return Unit.Value;
        }
    }
}
