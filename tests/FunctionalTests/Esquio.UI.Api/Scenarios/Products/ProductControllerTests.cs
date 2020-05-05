using Esquio;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Products.Add;
using Esquio.UI.Api.Shared.Models.Products.AddRing;
using Esquio.UI.Api.Shared.Models.Products.Details;
using Esquio.UI.Api.Shared.Models.Products.List;
using Esquio.UI.Api.Shared.Models.Products.Update;
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

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Products
{

    [Collection(nameof(AspNetCoreServer))]
    public class products_controller_should
    {
        private readonly ServerFixture _fixture;
        public products_controller_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task deletering_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.DeleteRing("productname", "ringName"))
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletering_response_forbiden_when_user_is_authenticated_but_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.DeleteRing("productname", "ringName"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletering_response_badrequest_if_product_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.DeleteRing("fooproduct", "barring"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletering_response_badrequest_if_ring_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.DeleteRing(product.Name, "barring"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletering_response_badrequest_if_ring_is_default()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.DeleteRing(product.Name, EsquioConstants.DEFAULT_RING_NAME))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }


        [Fact]
        [ResetDatabase]
        public async Task deletering_response_noconent_when_success()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var ring = Builders.Deployment()
                .WithName("production")
                .Build();

            product.Rings
                .Add(ring);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.DeleteRing(product.Name, ring.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task addring_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.AddRing("productname"))
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task addring_response_forbiden_when_user_is_authenticated_but_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.AddRing("productname"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task addring_response_badrequest_if_ring_name_is_not_valid()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.AddRing(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddRingRequest()
                  {
                      Name = "@#~invalidring@#~"
                  });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task addring_response_badrequest_if_ring_name_lower_than_5_characters()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.AddRing(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddRingRequest()
                  {
                      Name = "some"
                  });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task addring_response_badrequest_if_ring_name_grether_than_200_characters()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.AddRing(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddRingRequest()
                  {
                      Name = new string('c', 201)
                  });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task addring_response_created_when_success()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.AddRing(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddRingRequest()
                  {
                      Name = "Production"
                  });

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }


        [Fact]
        public async Task list_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.List())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
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

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListProductResponseDetail>>();

            content.Total
                .Should().Be(1);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(0);

            content.Items
                .First().Name
                .Should().BeEquivalentTo(product.Name);

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

            var product1 = Builders.Product()
               .WithName("fooproduct")
               .Build();

            var product2 = Builders.Product()
               .WithName("barproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product1, product2);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.List(pageIndex: 1, pageCount: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListProductResponseDetail>>();

            content.Total
                .Should().Be(2);

            content.Count
                .Should().Be(1);

            content.PageIndex
                .Should().Be(1);

            content.Items
                .Single().Name
                .Should().BeEquivalentTo(product1.Name);
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
                  .CreateRequest(ApiDefinitions.V3.Product.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListProductResponseDetail>>();

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
        public async Task list_response_forbiden_when_user_is_authenticated_but_not_authorized()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.List())
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

            var product = Builders.Product()
               .WithName("fooproduct")
               .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.List(pageIndex: 10, pageCount: 10))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<PaginatedResult<ListProductResponseDetail>>();

            content.Total
                .Should().Be(1);

            content.Count
                .Should().Be(0);

            content.PageIndex
                .Should().Be(10);

            content.Items
                .Count
                .Should().Be(0);
        }

        [Fact]
        public async Task add_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_product_name_already_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
              .WithName("fooproduct")
              .Build();

            await _fixture.Given
                .AddProduct(product);

            var productRequest = new AddProductRequest()
            {
                Name = product.Name,
                Description = "some description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_product_name_length_is_less_than_5()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = new string('c', 2),
                Description = "some description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_product_name_length_is_greater_than_200()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = new string('c', 201),
                Description = "some description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_description_length_is_greater_than_2000()
        {
            var permission = Builders.Permission()
            .WithManagementPermission()
            .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct",
                Description = new string('d', 2001)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_created_when_success()
        {
            var permission = Builders.Permission()
             .WithManagementPermission()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct",
                Description = "some description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status201Created);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_when_name_is_not_valid()
        {
            var permission = Builders.Permission()
             .WithManagementPermission()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct~#4",
                Description = "some description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_when_ringname_is_not_valid()
        {
            var permission = Builders.Permission()
             .WithManagementPermission()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct~#4",
                Description = "some description",
                DefaultRingName = "X"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task add_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
             .WithReaderPermission()
             .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct",
                Description = "some description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task get_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Get("product"))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_not_found_if_product_does_not_exist()
        {
            var permission = Builders.Permission()
              .WithManagementPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Get("product"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_ok_if_product_exist()
        {
            var permission = Builders.Permission()
              .WithManagementPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
             .WithName("fooproduct")
             .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Get(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status200OK);

            var content = await response.Content
                .ReadAs<DetailsProductResponse>();

            content.Name
                .Should().BeEquivalentTo(product.Name);
        }

        [Fact]
        [ResetDatabase]
        public async Task get_response_forbidden_if_user_is_not_authorized()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Get("product"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task delete_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Delete("product"))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_badrequest_if_product_not_exist()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Delete("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_no_content_when_delete_product_without_features()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
             .WithName("fooproduct")
             .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Delete(product.Name))
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

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Delete("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task delete_response_no_content_when_delete_product_with_features_toggles_and_parameters()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var ring = Builders.Deployment()
                .WithName(EsquioConstants.DEFAULT_RING_NAME)
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
                .WithRingName(ring.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            product.Rings
                .Add(ring);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Delete(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task update_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Update("fooproduct"))
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status401Unauthorized);
        }


        [Fact]
        [ResetDatabase]
        public async Task update_response_bad_request_if_product_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var request = new UpdateProductRequest()
            {
                Name = "fooproduct",
                Description = "description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Update("barproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        public async Task update_response_bad_request_if_product_name_is_less_than_5()
        {
            var permission = Builders.Permission()
                .WithContributorPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var request = new UpdateProductRequest()
            {
                Name = new string('n', 2),
                Description = "description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Update("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }
        [Fact]
        public async Task update_response_bad_request_if_product_name_is_greater_than_200()
        {
            var request = new UpdateProductRequest()
            {
                Name = new string('n', 201),
                Description = "description"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Update("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_bad_request_if_product_description_is_greater_than_2000()
        {
            var permission = Builders.Permission()
               .WithManagementPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var request = new UpdateProductRequest()
            {
                Name = "fooproduct",
                Description = new string('d', 2001)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Update("barproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_forbidden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var request = new UpdateProductRequest()
            {
                Name = "barpdroduct",
                Description = "description for product#2"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V3.Product.Update(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task update_response_no_content_if_product_is_updated()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var request = new UpdateProductRequest()
            {
                Name = "barproduct",
                Description = "description for product#2"
            };

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V3.Product.Update(product.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(StatusCodes.Status204NoContent);
        }
    }
}
