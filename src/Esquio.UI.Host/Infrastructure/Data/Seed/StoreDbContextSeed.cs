using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Host.Infrastructure.Data.Seed
{
    public static class StoreDbContextSeed
    {
        public static Action<StoreDbContext, IServiceProvider> Seed()
        {
            const string DEFAULT_SUBJECT_ID_CONFIGURATION_KEY = "Security:DefaultSubjectId";
            const string DEFAULT_USERS = "Security:DefaultUsers";
            
            return (context, sp) =>
            {
                if (!context.Permissions.Any())
                {
                    const string DEFAULT_API_KEY = "ZgZ9/qcwJGe/Utefuym5YS/84mE8/9x7kIrx2V/aIxc=";
                    var configuration = sp.GetRequiredService<IConfiguration>();

                    context.ApiKeys.Add(
                        new ApiKeyEntity("demo", DEFAULT_API_KEY, DateTime.UtcNow.AddYears(1)));
                    var defaultUsers = new List<PermissionEntity>();
                    foreach (var section in configuration.GetSection(DEFAULT_USERS)?.GetChildren()){
                        var entity = new PermissionEntity{
                            Kind = SubjectType.User
                        };
                        section.Bind(entity);
                        defaultUsers.Add(entity);
                    }
                    var defaultIdSvrPermission = new PermissionEntity()
                    {
                        SubjectId = configuration[DEFAULT_SUBJECT_ID_CONFIGURATION_KEY] ?? "1", //default alice user
                        ApplicationRole = ApplicationRole.Management,
                        Kind = SubjectType.User
                    };
                    defaultUsers.Add(defaultIdSvrPermission);

                    var apiKeyPermission = new PermissionEntity()
                    {
                        SubjectId = DEFAULT_API_KEY,
                        ApplicationRole = ApplicationRole.Reader,
                        Kind = SubjectType.Application
                    };
                    defaultUsers.Add(apiKeyPermission);
                    context.Permissions.AddRange(defaultUsers);
                    
                    context.SaveChanges();
                }


            };
        }
    }
}
