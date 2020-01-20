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

namespace Esquio.UI.Api.Scenarios.Users.Add
{
    public class AddPermissionRequestHandler
        : IRequestHandler<AddPermissionRequest>
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<AddPermissionRequestHandler> _logger;

        public AddPermissionRequestHandler(StoreDbContext storeDbContext,ILogger<AddPermissionRequestHandler> logger)
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

            if ( currentPermission  == null )
            {
                 _storeDbContext
                    .Permissions
                    .Add(new PermissionEntity()
                    {
                        SubjectId = request.SubjectId,
                        ReadPermission = request.Read,
                        WritePermission = request.Write,
                        ManagementPermission = request.Manage
                    });

                await _storeDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

            Log.SubjectIdAlreadyExist(_logger, request.SubjectId);
            throw new InvalidOperationException("SubjectId already exits on the store.");
        }
    }
}
