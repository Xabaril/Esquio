using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Permissions.Update;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Permissions.Update
{
    public class UpdatePermissionRequestHandler
        : IRequestHandler<UpdatePermissionRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<UpdatePermissionRequestHandler> _logger;

        public UpdatePermissionRequestHandler(StoreDbContext storeDbContext, ILogger<UpdatePermissionRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(UpdatePermissionRequest request, CancellationToken cancellationToken)
        {
            var currentPermission = await _storeDbContext
                .Permissions
                .Where(p => p.SubjectId == request.SubjectId)
                .SingleOrDefaultAsync(cancellationToken);

            if (currentPermission != null)
            {
                currentPermission.ApplicationRole = Enum.Parse<ApplicationRole>(request.ActAs, ignoreCase: true);

                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            Log.SubjectIdDoesNotExist(_logger, request.SubjectId);
            throw new InvalidOperationException("SubjectId does not exist in the store.");
        }
    }
}
