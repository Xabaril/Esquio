using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Esquio.UI.Infrastructure.Data.Seed
{
    public static class StoreDbContextSeed
    {
        public static Action<StoreDbContext, IServiceProvider> Seed()
        {
            const string DEFAULT_SUBJECT_ID_CONFIGURATION_KEY = "Security:DefaultSubjectId";

            return (context, sp) =>
            {
                if (!context.Permissions.Any())
                {
                    var configuration = sp.GetRequiredService<IConfiguration>();

                    var aliceIdSvrPermission = new PermissionEntity()
                    {
                        SubjectId = configuration[DEFAULT_SUBJECT_ID_CONFIGURATION_KEY] ?? "1",
                        ReadPermission = true,
                        WritePermission = true,
                        ManagementPermission = true,
                    };

                    context.Permissions.AddRange(aliceIdSvrPermission);

                    context.SaveChanges();
                }
            };
        }
    }
}
