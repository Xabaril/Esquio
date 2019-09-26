using System.Collections.Generic;
using System.Security.Claims;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class IdentityBuilder
    {
        public const string DEFAULT_NAME = "default-user";
        public const string DEFAULT_ROLE = "default-role";

        private List<Claim> _claims = new List<Claim>();

        public IdentityBuilder WithClaim(string type, string value)
        {
            _claims.Add(new Claim(type, value));
            return this;
        }

        public IdentityBuilder WithDefaultClaims()
        {
            _claims.Add(new Claim(ClaimTypes.Name, DEFAULT_NAME));
            _claims.Add(new Claim(ClaimTypes.NameIdentifier, DEFAULT_NAME));
            _claims.Add(new Claim(ClaimTypes.Role, DEFAULT_ROLE));
            _claims.Add(new Claim("iss", "TestServer issuer"));
            return this;
        }

        public IEnumerable<Claim> Build()
        {
            return _claims.AsReadOnly();
        }
    }
}

