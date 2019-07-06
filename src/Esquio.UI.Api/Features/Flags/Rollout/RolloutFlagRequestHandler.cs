using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Rollout
{
    internal class RolloutFlagRequestHandler
        : IRequestHandler<RolloutFlagRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<RolloutFlagRequestHandler> _logger;

        public RolloutFlagRequestHandler(StoreDbContext storeDbContext, ILogger<RolloutFlagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(RolloutFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(f => f.Id == request.FeatureId)
                .Include(f => f.Toggles)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Enabled = true;

                if ( !IsRolledout(feature))
                {
                    feature.Toggles.Clear();
                    feature.Toggles.Add(new ToggleEntity(feature.Id, nameof(Esquio.Toggles.OnToggle)));
                }

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            Log.FeatureNotExist(_logger, request.FeatureId.ToString());
            throw new InvalidOperationException("Feature does not exist in the store.");
        }

        bool IsRolledout(FeatureEntity feature)
        {
            return feature.Toggles.Count == 1
                && feature.Toggles.Single().Type.Equals(nameof(Esquio.Toggles.OnToggle), StringComparison.InvariantCulture);
        }
    }
}
