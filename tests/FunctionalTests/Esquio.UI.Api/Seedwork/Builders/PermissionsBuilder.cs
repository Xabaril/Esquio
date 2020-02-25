using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class PermissionsBuilder
    {
        ApplicationRole _applicationRole; 
        string _nameIdentifier = "default-user";

        public PermissionsBuilder WithNameIdentifier(string nameIdentifier)
        {
            _nameIdentifier = nameIdentifier;
            return this;
        }
       
        public PermissionsBuilder WithPermission(ApplicationRole role)
        {
            _applicationRole = role;
            return this;
        }

        public PermissionsBuilder WithReaderPermission()
        {
            _applicationRole = ApplicationRole.Reader;
            return this;
        }

        public PermissionsBuilder WithContributorPermission()
        {
            _applicationRole = ApplicationRole.Contributor;
            return this;
        }

        public PermissionsBuilder WithManagementPermission()
        {
            _applicationRole = ApplicationRole.Management;
            return this;
        }

     
        public PermissionEntity Build()
        {
            return new PermissionEntity()
            {
                Kind = SubjectType.User,
                ApplicationRole = _applicationRole,
                SubjectId = _nameIdentifier
            };
        }
    }
}
