using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Rollout
{
    internal class RolloutFlagRequestHandler
        : IRequestHandler<RolloutFlagRequest>
    {
        private readonly StoreDbContext _context;

        public RolloutFlagRequestHandler(StoreDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Unit> Handle(RolloutFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _context
                .Features
                .Where(f => f.Id == request.FeatureId)
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Toggles.Clear();
                feature.Toggles.Add(new ToggleEntity(feature.Id, nameof(Esquio.Toggles.OnToggle)));

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            throw new InvalidOperationException("Feature does not exist in the store.");
        }
    }
}
