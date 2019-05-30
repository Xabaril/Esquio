using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFlagRequestHandler : IRequestHandler<DeleteFlagRequest>
    {
        private readonly StoreDbContext _storeDbContext;

        public DeleteFlagRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }

        public async Task<Unit> Handle(DeleteFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(f => f.Id == request.FeatureId && f.ProductEntityId == request.ProductId, cancellationToken);

            if (feature != null)
            {
                _storeDbContext.Remove(feature);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException("Product or Feature doent's exist in the store.");
        }
    }
}
