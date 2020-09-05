using Esquio.UI.Client.Services;
using Esquio.UI.Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio.UI.Client.Infrastructure.Authorization
{
    class PolicyRequirementHandler : AuthorizationHandler<PolicyRequirement>
    {
        private readonly IEsquioHttpClient _esquioHttpClient;
        private readonly IPolicyBuilder _policyBuilder;
        private readonly EsquioState _state;
        private readonly ILogger<PolicyRequirementHandler> _logger;

        public PolicyRequirementHandler(
            IEsquioHttpClient esquioHttpClient,
            IPolicyBuilder policyBuilder,
            EsquioState state,
            ILogger<PolicyRequirementHandler> logger)
        {
            _esquioHttpClient = esquioHttpClient ?? throw new ArgumentNullException(nameof(esquioHttpClient));
            _policyBuilder = policyBuilder ?? throw new ArgumentNullException(nameof(policyBuilder));
            _state = state ?? throw new ArgumentNullException(nameof(_state));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override  async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (context.User != null && _state.LoggedUser == null)
            {
                var my = await _esquioHttpClient.GetMy();

                if ( my != null  && !String.IsNullOrEmpty(my.ActAs))
                {
                    var loggedUser = new LoggedUserViewModel()
                    {
                        UserName = context.User.Identity.Name,
                        SubjectId = context.User.FindFirst("sub").Value,
                        ActAs = my.ActAs
                    };

                    var policy = _policyBuilder.Build(my);

                    _state.ClearState();
                    _state.SetLoggedUser(loggedUser);
                    _state.SetPolicy(policy);
                }
                else
                {
                    context.Fail();
                    return;
                }
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
        }

        private void LogAuthorizationFailed(string subjectId, string permission)
        {
            _logger.LogWarning($"Authorization failed for user {subjectId} with required permission {permission}.");
        }
    }
}
