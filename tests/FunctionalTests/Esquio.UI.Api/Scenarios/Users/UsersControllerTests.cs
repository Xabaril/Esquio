using Esquio.UI.Api.Features.Users.Add;
using Esquio.UI.Api.Features.Users.Details;
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
                 .CreateRequest(ApiDefinitions.V2.Users.My())
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
                 .CreateRequest(ApiDefinitions.V2.Users.My())
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
                 .CreateRequest(ApiDefinitions.V2.Users.My())
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
                 .CreateRequest(ApiDefinitions.V2.Users.List())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task details_response_unauthorized_if_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Details("some-subject-id"))
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task details_response_forbidden_if_user_is_authenticated_but_not_authorized()
        {
            var permission = Builders.Permission()
               .WithAllPrivilegesForDefaultIdentity()
               .WithManagementPermission(false)
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Details("some-subject-id"))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task details_response_bad_request_if_subject_id_is_greater_than_200()
        {
            var subjectId = new String('s', 201);

            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Details(subjectId))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task details_response_bad_request_if_subjectId_does_not_exist()
        {
            var subjectId = new String('s', 50);

            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Details(subjectId))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task details_response_ok_when_success()
        {
            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Details(permission.SubjectId))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<DetailsUsersResponse>();

            content.SubjectId
                .Should().BeEquivalentTo(permission.SubjectId);

            content.ReadPermission
                .Should().Be(permission.ReadPermission);

            content.WritePermission
                .Should().Be(permission.WritePermission);

            content.ManagementPermission
                .Should().Be(permission.ManagementPermission);
        }

        [Fact]
        public async Task delete_response_unauthorized_if_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Delete("some-subjectid"))
                 .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_forbidden_if_user_is_authenticated_but_not_authorized()
        {
            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .WithManagementPermission(false)
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Delete("some-subjectid"))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_bad_request_if_subject_id_is_greater_than_200()
        {
            var subjectId = new String('s', 201);

            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Delete(subjectId))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_bad_request_if_subjectId_does_not_exist()
        {
            var subjectId = new String('s', 50);

            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Delete(subjectId))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_ok_when_success()
        {
            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Delete(permission.SubjectId))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
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
                 .CreateRequest(ApiDefinitions.V2.Users.List())
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
                 .CreateRequest(ApiDefinitions.V2.Users.List(pageCount: -1))
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
                 .CreateRequest(ApiDefinitions.V2.Users.List(pageCount: 0))
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
                 .CreateRequest(ApiDefinitions.V2.Users.List(pageIndex: -1))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_whe_success_with_paginated_content()
        {
            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .WithManagementPermission(true)
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.List(pageIndex: 0, pageCount: 10))
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListUsersResponse>();

            content.Should()
                .NotBeNull();

            content.Result
                .Count.Should().Be(1);

            content.Result
                .First()
                .SubjectId.Should().Be(IdentityBuilder.DEFAULT_NAME);

            content.Result
               .First()
               .WritePermission.Should().BeTrue();

            content.Result
               .First()
               .ReadPermission.Should().BeTrue();

            content.Result
               .First()
               .ManagementPermission.Should().BeTrue();
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_success()
        {
            var permission = Builders.Permission()
                 .WithAllPrivilegesForDefaultIdentity()
                 .WithManagementPermission(true)
                 .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.List())
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListUsersResponse>();

            content.Should()
                .NotBeNull();

            content.Result
                .Count.Should().Be(1);

            content.Result
                .First()
                .SubjectId.Should().Be(IdentityBuilder.DEFAULT_NAME);

            content.Result
               .First()
               .WritePermission.Should().BeTrue();

            content.Result
               .First()
               .ReadPermission.Should().BeTrue();

            content.Result
               .First()
               .ManagementPermission.Should().BeTrue();
        }

        [Fact]
        public async Task add_response_unauthorized_if_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
                 .CreateRequest(ApiDefinitions.V2.Users.Add())
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
