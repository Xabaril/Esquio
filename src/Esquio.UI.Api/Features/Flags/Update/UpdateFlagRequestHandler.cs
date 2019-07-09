using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Flags.Update
{
    public class UpdateFlagRequestHandler : IRequestHandler<UpdateFlagRequest, int>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<UpdateFlagRequestHandler> _logger;

        public UpdateFlagRequestHandler(StoreDbContext storeDbContext, ILogger<UpdateFlagRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(UpdateFlagRequest request, CancellationToken cancellationToken)
        {
            var feature = await _storeDbContext
                .Features
                .Where(p => p.Id == request.FlagId)
                .SingleOrDefaultAsync(cancellationToken);

            if (feature != null)
            {
                feature.Name = request.Name;
                feature.Description = request.Description;
                feature.Enabled = request.Enabled;

                await _storeDbContext.SaveChangesAsync(cancellationToken);

                return feature.Id;
            }
            else
            {
                Log.FeatureNotExist(_logger, request.FlagId.ToString());
                throw new InvalidOperationException($"The feature with id {request.FlagId} does not exist in the store.");
            }
        }
    }
}
