using Esquio.EntityFrameworkCore.Store;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Features.Users.My
{
    public class MyRequestHandler
        : IRequestHandler<MyRequest, MyResponse>
    {
        private readonly StoreDbContext _storeDbContext;

        public MyRequestHandler(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
        }
        public async Task<MyResponse> Handle(MyRequest request, CancellationToken cancellationToken)
        {
            var permission = await _storeDbContext.Permissions
                .Where(p => p.SubjectId == request.SubjectId)
                .SingleOrDefaultAsync(cancellationToken);

            if (permission != null)
            {
                return MyResponse.With(
                    readPermission: permission.ReadPermission,
                    writePermission: permission.WritePermission,
                    managementPermission: permission.ManagementPermission);
            }

            return MyResponse.UnAuthorized();
        }
    }
}
