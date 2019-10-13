using Esquio.UI.Api.Features.Users.Add;
using Esquio.UI.Api.Features.Users.List;
using Esquio.UI.Api.Features.Users.My;
using Esquio.UI.Api.Features.Users.Update;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Users
{
    [Collection(nameof(AspNetCoreServer))]
    public class users_controller_should
    {
        private readonly ServerFixture _fixture;
        public users_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task my_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.My())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task my_response_forbidden_when_user_request_is_authenticated_but_user_does_not_have_permissions()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.My())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task my_response_permissions_when_authorized_user_request()
        {
            var permission = Builders.Permission()
               .WithAllPrivilegesForDefaultIdentity()
               .Build();

            await _fixture.Given
                .AddPermission(permission);


            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.My())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                 .ReadAs<MyResponse>();

            content.ReadPermission
                .Should().BeTrue();

            content.WritePermission
                .Should().BeTrue();

            content.ManagementPermission
                .Should().BeTrue();
        }

        [Fact]
        public async Task list_response_unauthorized_if_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.List())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .WithManagementPermission(false)
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.List())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_badrequest_if_page_count_is_not_positive_number()
        {
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .WithManagementPermission(true)
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.List(pageCount:-1))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_badrequest_if_page_count_is_zero()
        {
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .WithManagementPermission(true)
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.List(pageCount: 0))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_badrequest_if_page_index_is_not_positive_number()
        {
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .WithManagementPermission(true)
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.List(pageIndex: -1))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok()
        {
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .WithManagementPermission(true)
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.List())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var  content = await response.Content
                .ReadAs<ListUsersResponse>();

            content.Should()
                .NotBeNull();

            content.UserPermissions
                .Count.Should().Be(1);

            content.UserPermissions
                .First()
                .SubjectId.Should().Be(IdentityBuilder.DEFAULT_NAME);

            content.UserPermissions
               .First()
               .WritePermission.Should().BeTrue();

            content.UserPermissions
               .First()
               .ReadPermission.Should().BeTrue();

            content.UserPermissions
               .First()
               .ManagementPermission.Should().BeTrue();
        }

        [Fact]
        public async Task add_response_unauthorized_if_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .PostAsJsonAsync(new AddPermissionRequest());

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .WithManagementPermission(false)
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PostAsJsonAsync(new AddPermissionRequest());

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_ok_when_success()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PostAsJsonAsync(new AddPermissionRequest()
                 {
                     SubjectId = Guid.NewGuid().ToString(),
                     Read = true,
                     Manage = true,
                     Write = true
                 });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_read_is_false_but_write_is_true()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PostAsJsonAsync(new AddPermissionRequest()
                 {
                     SubjectId = Guid.NewGuid().ToString(),
                     Read = false,
                     Manage = false,
                     Write = true
                 });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_write_is_false_but_management_is_true()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PostAsJsonAsync(new AddPermissionRequest()
                 {
                     SubjectId = Guid.NewGuid().ToString(),
                     Read = true,
                     Manage = true,
                     Write = false
                 });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .WithManagementPermission(false)
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PutAsJsonAsync(new UpdatePermissionRequest());

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task update_response_unauthorized_if_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .PostAsJsonAsync(new UpdatePermissionRequest());

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_ok_when_success()
        {
            var requesterPermission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            var existingPermissionSubjectId = Guid.NewGuid().ToString();

            var existingPermission = Builders.Permission()
              .WithNameIdentifier(existingPermissionSubjectId)
              .WithReadPermission(true)
              .WithWritePermission(false)
              .WithManagementPermission(false)
              .Build();

            await _fixture.Given
                .AddPermission(requesterPermission, existingPermission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PutAsJsonAsync(new AddPermissionRequest()
                 {
                     SubjectId = existingPermissionSubjectId,
                     Read = true,
                     Manage = true,
                     Write = true
                 });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_badrequest_if_read_is_false_but_write_is_true()
        {
            var requesterPermission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            var existingPermissionSubjectId = Guid.NewGuid().ToString();

            var existingPermission = Builders.Permission()
              .WithNameIdentifier(existingPermissionSubjectId)
              .WithReadPermission(true)
              .WithWritePermission(false)
              .WithManagementPermission(false)
              .Build();

            await _fixture.Given
                .AddPermission(requesterPermission, existingPermission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PutAsJsonAsync(new AddPermissionRequest()
                 {
                     SubjectId = existingPermissionSubjectId,
                     Read = false,
                     Manage = false,
                     Write = true
                 });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_badrequest_if_write_is_false_but_management_is_true()
        {
            var requesterPermission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            var existingPermissionSubjectId = Guid.NewGuid().ToString();

            var existingPermission = Builders.Permission()
              .WithNameIdentifier(existingPermissionSubjectId)
              .WithReadPermission(true)
              .WithWritePermission(false)
              .WithManagementPermission(false)
              .Build();

            await _fixture.Given
                .AddPermission(requesterPermission, existingPermission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V1.Users.Add())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .PutAsJsonAsync(new AddPermissionRequest()
                 {
                     SubjectId = existingPermissionSubjectId,
                     Read = false,
                     Manage = true,
                     Write = false
                 });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

    }
}
