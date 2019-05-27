using System.Collections.Generic;
using System.Security.Claims;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class IdentityBuilder
    {
        private List<Claim> _claims = new List<Claim>();

        public IdentityBuilder WithClaim(string type, string value)
        {
            _claims.Add(new Claim(type, value));
            return this;
        }

        public IdentityBuilder WithDefaultClaims()
        {
            _claims.Add(new Claim(ClaimTypes.Name, "default-user"));
            _claims.Add(new Claim(ClaimTypes.Role, "default-role"));
            return this;
        }

        public IEnumerable<Claim> Build()
        {
            return _claims.AsReadOnly();
        }
    }
}
