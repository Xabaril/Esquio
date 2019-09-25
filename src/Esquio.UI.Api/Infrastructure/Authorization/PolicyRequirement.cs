using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;

namespace Esquio.UI.Api.Infrastructure.Authorization
{
    internal class PolicyRequirement
        : IAuthorizationRequirement
    {
        public string ClaimType => ClaimTypes.NameIdentifier;

        public string Permission { get; }

        public PolicyRequirement(string permission)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}
