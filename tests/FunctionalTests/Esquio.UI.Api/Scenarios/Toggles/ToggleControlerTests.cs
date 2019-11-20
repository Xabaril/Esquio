using Esquio;
using Esquio.AspNetCore.Toggles;
using Esquio.Toggles;
using Esquio.UI.Api.Features.Toggles.Add;
using Esquio.UI.Api.Features.Toggles.AddParameter;
using Esquio.UI.Api.Features.Toggles.Details;
using Esquio.UI.Api.Features.Toggles.KnownTypes;
using Esquio.UI.Api.Features.Toggles.Reveal;
using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
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
                  .CreateRequest(ApiDefinitions.V2.Toggles.Get(productName:"foo",featureName:"bar",toggleType:"type"))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_not_found_if_toggle_not_exist()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Get(productName: product.Name, featureName: feature.Name, toggleType: "non-existing-toggle"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_ok_and_contain_details()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("Esquio.Toggles.FromToToggle, Esquio")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("From")
                .WithValue("01/91/2991")
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
                  .CreateRequest(ApiDefinitions.V2.Toggles.Get(product.Name,feature.Name,toggle.Type))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<DetailsToggleResponse>();

            content.Type
                .Should()
                .BeEquivalentTo("Esquio.Toggles.FromToToggle, Esquio");

            content.Assembly
                .Should()
                .BeEquivalentTo("Esquio");

            content.Parameters
                .Count
                .Should().Be(1);

            content.Parameters
                .First()
                .Name
                .Should().Be("From");

            content.Parameters
                .First()
                .Value
                .Should().Be("01/91/2991");
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_badrequest_if_configured_type_is_not_valid()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("NonExistingTYpe")
              .Build();

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Get(product.Name,feature.Name,toggle.Type))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task get_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .WithReadPermission(false)
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Get(productName: "foo", featureName: "bar", toggleType: "type"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task delete_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Delete(productName: "fooproduct", featureName: "barfeature", toggleType: "type"))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .WithWritePermission(false)
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Delete(productName:"fooproduct", featureName:"barfeature", toggleType: "type"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_badrequest_if_toggle_not_exist()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
              .WithName("fooproduct")
              .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Delete(product.Name, feature.Name, "non-existing"))
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
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("toggle-type-1")
              .Build();

            var parameter = Builders.Parameter()
                .WithName("param1")
                .WithValue("value1")
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
                  .CreateRequest(ApiDefinitions.V2.Toggles.Delete(product.Name, feature.Name, toggle.Type))
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
                  .CreateRequest(ApiDefinitions.V2.Toggles.Reveal("Esquio.Toggles.FromToToggle"))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }


        [Fact]
        [ResetDatabase]
        public async Task reveal_response_badrequest_if_toggle_not_exist()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Reveal("Esquio.Toggles.NonExisting"))
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
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("product#1")
               .Build();

            var feature = Builders.Feature()
                .WithName("feature#1")
                .Build();

            var toggle = Builders.Toggle()
              .WithType("Esquio.Toggles.EnvironmentToggle, Esquio")
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
                  .CreateRequest(ApiDefinitions.V2.Toggles.Reveal("Esquio.Toggles.EnvironmentToggle, Esquio"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<RevealToggleResponse>();

            content.Type
                .Should()
                .BeEquivalentTo("Esquio.Toggles.EnvironmentToggle, Esquio");

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
                .Should().BeEquivalentTo(EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE);

            content.Parameters
                .First()
                .Description
                .Should().NotBeNull();
        }
        [Fact]
        [ResetDatabase]
        public async Task reveal_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .WithReadPermission(false)
             .Build();

            await _fixture.Given
                .AddPermission(permission);

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
                  .CreateRequest(ApiDefinitions.V2.Toggles.Reveal("Esquio.Toggles.EnvironmentToggle"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task knowntypes_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.KnownTypes())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task knowntypes_response_forbidden_if_user_is_not_authorize()
        {
            var permission = Builders.Permission()
               .WithAllPrivilegesForDefaultIdentity()
               .WithReadPermission(false)
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.KnownTypes())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task knowntypes_response_ok()
        {
            var permission = Builders.Permission()
               .WithAllPrivilegesForDefaultIdentity()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.KnownTypes())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<KnownTypesToggleResponse>();

            content.Toggles
                .Where(t => t.Assembly == (typeof(FromToToggle).Assembly.GetName().Name))
                .Any().Should().BeTrue();

            content.Toggles
              .Where(t => t.Type == (typeof(FromToToggle).ShorthandAssemblyQualifiedName()))
              .Any().Should().BeTrue();

            content.Toggles
              .Where(t => t.Type == (typeof(FromToToggle).ShorthandAssemblyQualifiedName()))
              .Select(t => t.Description)
              .Single()
              .Should().BeEquivalentTo("Toggle that is active depending on current UTC date.");

            content.Toggles
              .Where(t => t.Type == (typeof(FromToToggle).ShorthandAssemblyQualifiedName()))
              .Select(t => t.FriendlyName)
              .Single()
              .Should().BeEquivalentTo("Between dates");

            content.Toggles
              .Where(t => t.Type == (typeof(ClaimValueToggle).ShorthandAssemblyQualifiedName()))
              .Select(t => t.Description)
              .Single()
              .Should().BeEquivalentTo("Toggle that is active depending on the current claims of authenticated users.");
        }

        [Fact]
        public async Task add_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Post())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_bad_request_if_try_to_duplicate_toggle()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
             .WithName("fooproduct")
             .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType(typeof(EnvironmentToggle).FullName)
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

            var body = new AddToggleRequest()
            {
                ProductName = product.Name,
                FeatureName = feature.Name,
                ToggleType = typeof(EnvironmentToggle).FullName,
                Parameters = new List<AddToggleRequestDetailParameter>()
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Post())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(body);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_created_when_add_new_toggle()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
             .WithName("fooproduct")
             .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var body = new AddToggleRequest()
            {
                ProductName = product.Name,
                FeatureName = feature.Name,
                ToggleType = typeof(EnvironmentToggle).FullName,
                Parameters = new List<AddToggleRequestDetailParameter>()
                {
                    new AddToggleRequestDetailParameter()
                    {
                        Name = "Environments",
                        Value = "Development",
                        Type = typeof(String).FullName
                    }
                }
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.Post())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(body);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task addparameter_response_unauthorized_when_user_request_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_bad_request_if_name_is_null()
        {
            var permission = Builders.Permission()
            .WithAllPrivilegesForDefaultIdentity()
            .Build();

            await _fixture.Given
                .AddPermission(permission);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = "fooproduct",
                FeatureName = "barfeature",
                ToggleType = "type",
                Name = null,
                Value = "Development"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_bad_request_if_name_is_empty()
        {
            var permission = Builders.Permission()
             .WithAllPrivilegesForDefaultIdentity()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = "fooproduct",
                FeatureName = "barfeature",
                ToggleType = "type",
                Name = string.Empty,
                Value = "Development"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_bad_request_if_value_is_null()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = "fooproduct",
                FeatureName = "barfeature",
                ToggleType = "type",
                Name = "Environment",
                Value = null
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_bad_request_if_value_is_empty()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = "fooproduct",
                FeatureName = "barfeature",
                ToggleType = "type",
                Name = "Environment",
                Value = string.Empty
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }


        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_badrequest_if_toggle_does_not_exist()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = "fooproduct",
                FeatureName = "barfeature",
                ToggleType = "type",
                Name = "Environments",
                Value = "Development"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_ok_if_toggle_exist_but_is_new_parameter()
        {
            var permission = Builders.Permission()
                .WithAllPrivilegesForDefaultIdentity()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
             .WithName("fooproduct")
             .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType(typeof(EnvironmentToggle).FullName)
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

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = product.Name,
                FeatureName = feature.Name,
                ToggleType = toggle.Type,
                Name = "Environments",
                Value = "Production",
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }
        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_ok_if_toggle_and_parameter_already_exit()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
             .WithName("fooproduct")
             .Build();

            var feature = Builders.Feature()
                .WithName("barfeature")
                .Build();

            var toggle = Builders.Toggle()
              .WithType(typeof(EnvironmentToggle).FullName)
              .Build();

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            await _fixture.Given
                .AddProduct(product);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = product.Name,
                FeatureName = feature.Name,
                ToggleType = toggle.Type,
                Name = "Environments",
                Value = "Development",
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }
        [Fact]
        [ResetDatabase]
        public async Task addparameter_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
              .WithAllPrivilegesForDefaultIdentity()
              .WithWritePermission(false)
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var parameterToggleRequest = new AddParameterToggleRequest()
            {
                ProductName = "fooproduct",
                FeatureName = "barfeature",
                ToggleType = "type",
                Name = "Environments",
                Value = "Development",
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V2.Toggles.PostParameter())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(parameterToggleRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

    }
}
