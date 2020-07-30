using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Shared.Models.Permissions.My;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Permissions.My
{
    public class MyRequestHandler
        : IRequestHandler<MyRequest, MyResponse>
    { 
        private readonly StoreDbContext _storeDbContext;
        private readonly ILogger<MyRequestHandler> _logger;

        public MyRequestHandler(StoreDbContext storeDbContext, ILogger<MyRequestHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<MyResponse> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            var permission = await _storeDbContext.Permissions
                .Where(p => p.SubjectId == request.SubjectId)
                .SingleOrDefaultAsync(cancellationToken);

            if (permission != null)
            {
                return new MyResponse()
                {
                    ActAs = Enum.GetName(typeof(ApplicationRole), permission.ApplicationRole),
                    IsAuthorized = true,
                };
            }

            Log.MyIsNotAuthorized(_logger, request.SubjectId);
            return MyResponse.UnAuthorized();
        }
    }
}
