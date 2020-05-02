using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
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
                var subjectId = context.User
                    .GetSubjectId();

                if (subjectId != null)
                {
                    var currentPermission = await _storeDbContext.Permissions
                        .Where(p => p.SubjectId == subjectId)
                        .SingleOrDefaultAsync();

                    if (currentPermission != null)
                    {
                        bool allowed = requirement.Permission switch
                        {
                            Policies.Reader => (currentPermission.ApplicationRole & ApplicationRole.Reader) == ApplicationRole.Reader,
                            Policies.Contributor => (currentPermission.ApplicationRole & ApplicationRole.Contributor) == ApplicationRole.Contributor,
                            Policies.Management => (currentPermission.ApplicationRole & ApplicationRole.Management) == ApplicationRole.Management,
                            _ => throw new ArgumentNullException("The configured authorization policy is not supported.")
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
                    Log.AuthorizationFailClaimIsNotPressent(_logger, ApiConstants.SubjectNameIdentifier);
                    context.Fail();
                }
            }
        }
    }
}
