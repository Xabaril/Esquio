using Microsoft.AspNetCore.Authorization;
using System;

namespace Esquio.UI.Api.Infrastructure.Authorization
{
    internal class PolicyRequirement
        : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PolicyRequirement(string permission)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}
