using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class PermissionsBuilder
    {
        bool _readPermission = false;
        bool _writePermission = false;
        bool _managePermission = false;
        string _nameIdentifier = "default-user-id";

        public PermissionsBuilder WithNameIdentifier(string nameIdentifier)
        {
            _nameIdentifier = nameIdentifier;
            return this;
        }
        public PermissionsBuilder WithReadPermission(bool readPermisision)
        {
            _readPermission = readPermisision;
            return this;
        }
        public PermissionsBuilder WithWritePermission(bool writePermission)
        {
            _writePermission = writePermission;
            return this;
        }
        public PermissionsBuilder WithManagementPermission(bool managePermission)
        {
            _managePermission = managePermission;
            return this;
        }

        public PermissionsBuilder WithAllPrivilegesForDefaultIdentity()
        {
            _readPermission = true;
            _writePermission = true;
            _managePermission = true;
            _nameIdentifier = IdentityBuilder.DEFAULT_NAME;
            return this;
        }
        public PermissionEntity Build()
        {
            return new PermissionEntity()
            {
                ReadPermission = _readPermission,
                WritePermission = _writePermission,
                ManagementPermission = _managePermission,
                SubjectId = _nameIdentifier
            };
        }
    }
}
