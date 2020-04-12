using Esquio.UI.Api.Infrastructure.Data.Entities;
using Esquio.UI.Api.Scenarios.ApiKeys.Add;
using Esquio.UI.Api.Scenarios.ApiKeys.Details;
using Esquio.UI.Api.Scenarios.ApiKeys.List;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.ApiKeys.Add;
using Esquio.UI.Api.Shared.Models.ApiKeys.Details;
using Esquio.UI.Api.Shared.Models.ApiKeys.List;
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

namespace FunctionalTests.Esquio.UI.Api.Scenarios.ApiKeys
{
    [Collection(nameof(AspNetCoreServer))]
    public class apikeys_controller_should
    {
        private readonly ServerFixture _fixture;
        public apikeys_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task get_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Get(name: "fooname"))
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }


        [Fact]
        [ResetDatabase]
        public async Task get_response_notfound_when_apikeyid_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Get(name: "fooname"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_apikey_when_exist()
        {
            var apiKeyValue = "barkey";
            var apiKeyName = "fooname";

            var requesterPermission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            var apiKeyPermission = Builders.Permission()
                .WithManagementPermission()
                .WithNameIdentifier(apiKeyValue)
                .Build();

            await _fixture.Given
                .AddPermission(requesterPermission, apiKeyPermission);

            var apiKey = Builders.ApiKey()
               .WithName(apiKeyName)
               .Withkey(apiKeyValue)
               .WithValidTo(DateTime.UtcNow.AddYears(1))
               .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Get(apiKey.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            var content = await response.Content
                .ReadAs<DetailsApiKeyResponse>();

            content.Name
                .Should()
                .BeEquivalentTo(apiKeyName);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var apiKey = Builders.ApiKey()
               .WithName("fooname")
               .Withkey("barkey")
               .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Get(apiKey.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_default_skip_take_values()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var apiKey1 = Builders.ApiKey()
               .WithName("apikey#1")
               .Withkey("key-1")
               .WithValidTo(DateTime.UtcNow.AddYears(1))
               .Build();

            var apiKey2 = Builders.ApiKey()
              .WithName("apikey#2")
              .Withkey("key-2")
              .WithValidTo(DateTime.UtcNow.AddYears(1))
              .Build();

            await _fixture.Given
                .AddApiKey(apiKey1, apiKey2);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListApiKeyResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(2);

            content.PageIndex
                .Should().Be(0);

            content.Items
                .First().Name
                .Should().BeEquivalentTo("apikey#1");

            content.Items
                .Last().Name
                .Should().BeEquivalentTo("apikey#2");
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_specific_skip_take_values()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var apiKey1 = Builders.ApiKey()
               .WithName("apikey#1")
               .Withkey("key-1")
               .WithValidTo(DateTime.UtcNow.AddYears(1))
               .Build();

            var apiKey2 = Builders.ApiKey()
              .WithName("apikey#2")
              .Withkey("key-2")
              .WithValidTo(DateTime.UtcNow.AddYears(1))
              .Build();

            await _fixture.Given
                .AddApiKey(apiKey1, apiKey2);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.List(pageIndex: 1, pageCount: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListApiKeyResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(1);

            content.Items
                .Single().Name
                .Should().BeEquivalentTo("apikey#2");
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_data()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListApiKeyResponseDetail>>();

            content.Total
                .Should().Be(0);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(0);

            content.Items
                .Count
                .Should().Be(0);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
               .WithReaderPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_page_data()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var apiKey1 = Builders.ApiKey()
                .WithName("apikey#1")
                .Withkey("key-1")
                .WithValidTo(DateTime.UtcNow.AddYears(1))
                .Build();

            var apiKey2 = Builders.ApiKey()
              .WithName("apikey#2")
              .Withkey("key-2")
              .WithValidTo(DateTime.UtcNow.AddYears(1))
              .Build();

            await _fixture.Given
                .AddApiKey(apiKey1, apiKey2);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.List(pageIndex: 10, pageCount: 10))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListApiKeyResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(10);

            content.Items
                .Count
                .Should().Be(0);
        }

        [Fact]
        public async Task add_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_name_is_greater_than_200()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = new string('c', 201),
                ActAs = nameof(ApplicationRole.Reader)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_actas_is_not_enum_valid()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = new string('c', 100),
                ActAs = "NewRole"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = "apikey#1",
                ValidTo = DateTime.UtcNow.AddYears(2),
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_ok_and_use_default_validTo_if_is_not_specified()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = "fooname",
                ActAs = nameof(ApplicationRole.Reader)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);

            var content = await response.Content
                .ReadAs<AddApiKeyResponse>();

            content.Name
                .Should()
                .Be(addApiKeyRequest.Name);

            content.Key
                .Should()
                .NotBe(default);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_apikey_with_the_same_name_already_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var apiKey1 = Builders.ApiKey()
              .WithName("apikey#1")
              .Withkey("key-1")
              .Build();

            var apiKey2 = Builders.ApiKey()
              .WithName("apikey#2")
              .Withkey("key-2")
              .Build();

            await _fixture.Given
                .AddApiKey(apiKey1, apiKey2);

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = "apikey#1",
                ActAs = "Reader",
                ValidTo = DateTime.UtcNow.AddYears(1)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task add_response_created_when_create_new_apikey()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = "fookey",
                ActAs = nameof(ApplicationRole.Reader),
                ValidTo = DateTime.UtcNow.AddYears(2),
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);

            var content = await response.Content
               .ReadAs<AddApiKeyResponse>();

            content.Name
                .Should()
                .Be(addApiKeyRequest.Name);

            content.Key
                .Should()
                .NotBe(default);
        }

        [Fact]
        public async Task delete_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Delete(name: "fooname"))
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_bad_request_if_api_key_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Delete(name: "fooname"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_no_content_when_apikey_is_removed()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey("barkey")
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var apikeyPermission = Builders.Permission()
                .WithManagementPermission()
                .WithNameIdentifier("barkey")
                .WithSubjectType(SubjectType.Application)
                .Build();

            await _fixture.Given
                .AddPermission(apikeyPermission, permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Delete(apiKey.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_forbidden_when_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var apiKey = Builders.ApiKey()
                .WithName("fooname")
                .Withkey("barkey")
                .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.ApiKeys.Delete(apiKey.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }
    }
}
