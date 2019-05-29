using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Toggles.Delete
{
    public class DeleteToggleRequestHandler : IRequestHandler<DeleteToggleRequest>
    {
        private readonly StoreDbContext _storeDbContext;

        public DeleteToggleRequestHandler(StoreDbContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            _storeDbContext = context;
        }

        public async Task<Unit> Handle(DeleteToggleRequest request, CancellationToken cancellationToken)
        {
            var toggle = await _storeDbContext
                .Toggles
                .Where(t => t.Id == request.ToggleId)
                .SingleOrDefaultAsync(cancellationToken);

            if (toggle != null)
            {
                _storeDbContext.Remove(toggle);
                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException("Specified toggle doent's exist.");
        }
    }
}
