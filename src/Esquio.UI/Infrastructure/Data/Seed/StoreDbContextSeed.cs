using Esquio.EntityFrameworkCore.Store;
using Esquio.EntityFrameworkCore.Store.Entities;
using System;
using System.Linq;

namespace Esquio.UI.Infrastructure.Data.Seed
{
    public static class StoreDbContextSeed
    {
        public static Action<StoreDbContext,IServiceProvider> Seed()
        {
            return (context, sp) =>
            {
                if (!context.Permissions.Any())
                {
                    var aliceIdSvrPermission = new PermissionEntity()
                    {
                        SubjectId = "1",
                        ReadPermission = true,
                        WritePermission = true,
                        ManagementPermission = true,
                    };

                    var unaiGoogleAccount = new PermissionEntity()
                    {
                        SubjectId = "65ad4d0aaa2941d03cce9cc9f595f55a6b30eddb068d8bf6b61fbd2599045953",
                        ReadPermission = true,
                        WritePermission = true,
                        ManagementPermission = true,
                    };
                    
                    context.Permissions.AddRange(aliceIdSvrPermission, unaiGoogleAccount);

                    context.SaveChanges();
                }
            };
        }
    }
}
