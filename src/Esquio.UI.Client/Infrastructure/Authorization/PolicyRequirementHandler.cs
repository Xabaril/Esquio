using Esquio.UI.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio.UI.Client.Infrastructure.Authorization
{
    class PolicyRequirementHandler : AuthorizationHandler<PolicyRequirement>
    {
        private readonly EsquioState _state;
        private readonly ILogger<PolicyRequirementHandler> _logger;

        public PolicyRequirementHandler(EsquioState state, ILogger<PolicyRequirementHandler> logger)
        {
            _state = state ?? throw new ArgumentNullException(nameof(_state));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (_state.LoggedUser == null)
            {
                _logger.LogError("Authorization failed because the logged user is not present.");
                context.Fail();
                return Task.CompletedTask;
            }

            if (string.IsNullOrEmpty(_state.LoggedUser.ActAs))
            {
                LogAuthorizationFailed(_state.LoggedUser.SubjectId, requirement.Permission);
                context.Fail();
                return Task.CompletedTask;
            }

            var actAs = ActAs.From(_state.LoggedUser.ActAs);

            bool allowed = requirement.Permission switch
            {
                Policies.Reader => actAs == ActAs.Reader || actAs == ActAs.Contributor || actAs == ActAs.Management,
                Policies.Contributor => actAs == ActAs.Contributor || actAs == ActAs.Management,
                Policies.Management => actAs == ActAs.Management,
                _ => throw new ArgumentNullException("The configured authorization policy is not supported.")
            };

            if (!allowed)
            {
                LogAuthorizationFailed(_state.LoggedUser.SubjectId, requirement.Permission);
                context.Fail();
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private void LogAuthorizationFailed(string subjectId, string permission)
        {
            _logger.LogWarning($"Authorization failed for user {subjectId} with required permission {permission}.");
        }
    }
}
