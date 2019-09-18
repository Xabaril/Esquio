using Esquio.UI.Api.Features.ApiKeys.Add;
using Esquio.UI.Api.Features.ApiKeys.Details;
using Esquio.UI.Api.Features.ApiKeys.List;
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
                .CreateRequest(ApiDefinitions.V1.ApiKeys.Get(apiKeyId: 1))
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task get_response_notfound_when_apikeyid_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.ApiKeys.Get(apiKeyId: -11))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_notfound_when_apikeyid_does_not_exist()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.ApiKeys.Get(apiKeyId:11))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task get_response_apikey_when_exist()
        {
            var apiKey = Builders.ApiKey()
               .WithName("apikey#1")
               .Withkey("key-1")
               .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V1.ApiKeys.Get(apiKey.Id))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            var content = await response.Content
                .ReadAs<DetailsApiKeyResponse>();

            content.Name
                .Should()
                .BeEquivalentTo("apikey#1");

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_default_skip_take_values()
        {
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
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListApiKeyResponse>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(2);

            content.PageIndex
                .Should().Be(0);

            content.Result
                .First().Name
                .Should().BeEquivalentTo("apikey#1");

            content.Result
                .Last().Name
                .Should().BeEquivalentTo("apikey#2");
        }

        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_and_use_specific_skip_take_values()
        {
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
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.List(pageIndex: 1, pageCount: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListApiKeyResponse>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(1);

            content.Result
                .Single().Name
                .Should().BeEquivalentTo("apikey#2");
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_data()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListApiKeyResponse>();

            content.Total
                .Should().Be(0);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(0);

            content.Result
                .Count
                .Should().Be(0);
        }
        [Fact]
        [ResetDatabase]
        public async Task list_response_ok_when_no_page_data()
        {
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
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.List(pageIndex: 10, pageCount: 10))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<ListApiKeyResponse>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(10);

            content.Result
                .Count
                .Should().Be(0);
        }

        [Fact]
        public async Task add_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Add())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task add_response_badrequest_if_name_is_greater_than_200()
        {
            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = new string('c', 201)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_ok_and_use_default_validTo_if_is_not_specified()
        {
            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = "name"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);

            var content = await response.Content
                .ReadAs<AddApiKeyResponse>();

            content.ApiKeyId
                .Should()
                .NotBe(default);

            content.ApiKey
                .Should()
                .NotBe(default);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_apikey_with_the_same_name_already_exist()
        {
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
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Add())
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

            var addApiKeyRequest = new AddApiKeyRequest()
            {
                Name = "apikey#1",
                ValidTo = DateTime.UtcNow.AddYears(2),
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(addApiKeyRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);

            var content = await response.Content
               .ReadAs<AddApiKeyResponse>();

            content.ApiKeyId
                .Should()
                .NotBe(default);

            content.ApiKey
                .Should()
                .NotBe(default);
        }

        [Fact]
        public async Task delete_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Delete(apiKeyId: 1))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_bad_request_if_api_key_does_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Delete(apiKeyId: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_notfound_if_apikey_id_is_not_positive_number()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Delete(apiKeyId: -1))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_no_content_when_apikey_is_removed()
        {
            var apiKey = Builders.ApiKey()
             .WithName("apikey#1")
             .Withkey("key-1")
             .Build();

            await _fixture.Given
                .AddApiKey(apiKey);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.ApiKeys.Delete(apiKey.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
    }
}
