using Esquio.EntityFrameworkCore.Store;
using Esquio.UI.Api.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Infrastructure.Authorization
{
    class PolicyRequirementHandler
        : AuthorizationHandler<PolicyRequirement>
    {
        private readonly ILogger<PolicyRequirementHandler> _logger;
        private readonly StoreDbContext _storeDbContext;

        public PolicyRequirementHandler(StoreDbContext storeDbContext, ILogger<PolicyRequirementHandler> logger)
        {
            _storeDbContext = storeDbContext ?? throw new ArgumentNullException(nameof(storeDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                //TODO: this hack is not so clear.. the assumption name is a trick

                if(context.User.Identity.AuthenticationType.Equals("ApiKey",StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Succeed(requirement);

                    return;
                }

                var subjectId = context.User
                    .FindFirst(requirement.ClaimType)
                    .Value;

                if (subjectId != null)
                {
                    var currentPermission = await _storeDbContext.Permissions
                        .Where(p => p.SubjectId == subjectId)
                        .SingleOrDefaultAsync();

                    if (currentPermission != null)
                    {
                        bool allowed = requirement.Permission switch
                        {
                            Policies.Read => currentPermission.ReadPermission,
                            Policies.Write => currentPermission.WritePermission,
                            Policies.Management => currentPermission.ManagementPermission,
                            _ => throw new ArgumentNullException("Permission evaluation not supported.")
                        };

                        if (!allowed)
                        {
                            Log.AuthorizationRequiredPermissionFail(_logger, subjectId, requirement.Permission);
                            context.Fail();
                        }

                        context.Succeed(requirement);
                    }
                    else
                    {
                        Log.AuthorizationFail(_logger, subjectId);
                        context.Fail();
                    }
                }
                else
                {
                    Log.AuthorizationFailClaimIsNotPressent(_logger, requirement.ClaimType);
                    context.Fail();
                }
            }
        }
    }
}
