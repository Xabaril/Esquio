using Esquio.EntityFrameworkCore.Store;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Delete
{
    public class DeleteFlagRequestHandler : IRequestHandler<DeleteFlagRequest>
    {
        private readonly StoreDbContext _context;

        public DeleteFlagRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));
            _context = context;
        }

        public async Task<Unit> Handle(DeleteFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _context.GetFlagBy(request.ProductId, request.FlagId);

            if (feature != null)
            {
                throw new InvalidOperationException("Product or Feature doent's exist in the store.");
            }

            _context.Remove(feature);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
