using Esquio.UI.Api.Features.Toggles.Details;
using Esquio.UI.Api.Features.Toggles.Reveal;
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

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Toggles
{
    [Collection(nameof(AspNetCoreServer))]
    public class toggles_controller_should
    {
        private readonly ServerFixture _fixture;
        public toggles_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task get_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Get(toggleId: 1))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }
        [Fact]
        public async Task get_response_not_found_if_toggle_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Get(toggleId: -1))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task get_response_not_found_if_toggle_not_exist()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var toggle1 = Builders.Toggle()
              .WithType("toggle-type-1")
              .Build();

            feature.Toggles
                .Add(toggle1);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Get(toggleId: toggle1.Id * 2))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_ok_if_toggle_exist()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("toggle-type-1")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("param#1")
                .WithValue("value#1")
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Get(toggleId: toggle.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<DetailsToggleResponse>();

            content.TypeName
                .Should()
                .BeEquivalentTo("toggle-type-1");

            content.Parameters
                .Count
                .Should().Be(1);

            content.Parameters
                .First()
                .Key
                .Should().BeEquivalentTo("param#1");

            content.Parameters
                .First()
                .Value
                .Should().BeEquivalentTo("value#1");
        }

        [Fact]
        public async Task delete_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Delete(toggleId: 1))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task delete_response_badrequest_if_toggle_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Delete(toggleId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_badrequest_if_toggle_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Delete(toggleId: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_no_content_if_toggle_exist()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("toggle-type-1")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("param#1")
                .WithValue("value#1")
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Delete(toggleId: toggle.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
        [Fact]
        public async Task reveal_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Reveal(toggleId: 1))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task reveal_response_badrequest_if_toggle_is_not_positive_int()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Reveal(toggleId: -1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task reveal_response_badrequest_if_toggle_not_exist()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Reveal(toggleId: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task reveal_response_ok_if_toggle_exist_and_parameters_can_be_revealed()
        {
            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("Esquio.Toggles.EnvironmentToggle")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("Environments")
                .WithValue("Development")
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V1.Toggles.Reveal(toggleId: toggle.Id))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<RevealToggleResponse>();

            content.Type
                .Should()
                .BeEquivalentTo("Esquio.Toggles.EnvironmentToggle");

            content.Parameters
                .Count
                .Should().Be(1);

            content.Parameters
                .First()
                .Name
                .Should().BeEquivalentTo("Environments");

            content.Parameters
                .First()
                .ClrType
                .Should().BeEquivalentTo(typeof(String).FullName);

            content.Parameters
                .First()
                .Description
                .Should().NotBeNull();
        }
    }
}
