using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Permissions.Add;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Permissions.Add
{
    public class AddPermissionRequestHandler
        : IRequestHandler<AddPermissionRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddPermissionRequestHandler> _logger;

        public AddPermissionRequestHandler(StoreDbContext storeDbContext, ILogger<AddPermissionRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(AddPermissionRequest request, CancellationToken cancellationToken)
        {
            var currentPermission = await _storeDbContext
                .Permissions
                .Where(p => p.SubjectId == request.SubjectId)
                .SingleOrDefaultAsync(cancellationToken);

            if (currentPermission == null)
            {
                _storeDbContext
                   .Permissions
                   .Add(new PermissionEntity()
                   {
                       SubjectId = request.SubjectId,
                       ApplicationRole = Enum.Parse<ApplicationRole>(request.ActAs, ignoreCase: true)
                   });

                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            Log.SubjectIdAlreadyExist(_logger, request.SubjectId);
            throw new InvalidOperationException("The permission for this SubjectId already exits on the store.");
        }
    }
}
