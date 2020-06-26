using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Store.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Esquio.UI.Host.Infrastructure.Data.Seed
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
                    const string DEFAULT_API_KEY = "ZgZ9/qcwJGe/Utefuym5YS/84mE8/9x7kIrx2V/aIxc=";
                    var configuration = sp.GetRequiredService<IConfiguration>();

                    context.ApiKeys.Add(
                        new ApiKeyEntity("demo", DEFAULT_API_KEY, DateTime.UtcNow.AddYears(1)));

                    var aliceIdSvrPermission = new PermissionEntity()
                    {
                        SubjectId = configuration[DEFAULT_SUBJECT_ID_CONFIGURATION_KEY] ?? "1",
                        ApplicationRole = ApplicationRole.Management
                    };

                    var bobIdSvrPermission = new PermissionEntity()
                    {
                        SubjectId = configuration[DEFAULT_SUBJECT_ID_CONFIGURATION_KEY] ?? "11",
                        ApplicationRole = ApplicationRole.Reader
                    };

                    var apiKeyPermission = new PermissionEntity()
                    {
                        SubjectId = DEFAULT_API_KEY,
                        ApplicationRole = ApplicationRole.Reader
                    };

                    context.Permissions.AddRange(aliceIdSvrPermission, bobIdSvrPermission);

                    context.SaveChanges();
                }

                
            };
        }
    }
}
