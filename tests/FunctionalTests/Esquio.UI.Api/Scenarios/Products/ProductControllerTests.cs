﻿using Esquio;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Products.Add;
using Esquio.UI.Api.Shared.Models.Products.AddDeployment;
using Esquio.UI.Api.Shared.Models.Products.Details;
using Esquio.UI.Api.Shared.Models.Products.Import;
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
using System.Net;
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
        [ResetDatabase]
        public async Task importproduct_response_ok_when_success()
        {
            var permission = Builders.Permission()
              .WithContributorPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Import())
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(new ImportProductRequest()
                {
                    Content = "{\"Name\":\"default\",\"Description\":\"this is default product\",\"Features\":[{\"Name\":\"MatchScore\",\"Description\":\"show the match score on products\",\"Archived\":false,\"FeatureTags\":[],\"FeatureStates\":[],\"Toggles\":[{\"Type\":\"Esquio.Toggles.FromToToggle,Esquio\",\"Parameters\":[{\"Name\":\"From\",\"Value\":\"2020-07-13 09:36:23\",\"DeploymentName\":\"Tests\"},{\"Name\":\"To\",\"Value\":\"2020-07-29 09:36:25\",\"DeploymentName\":\"Tests\"}]}]}],\"Deployments\":[{\"Name\":\"Tests\",\"ByDefault\":true},{\"Name\":\"production\",\"ByDefault\":false}]}"
                });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task importproduct_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Import())
                .PostAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task importproduct_response_bad_request_if_product_to_import_already_exists()
        {
            var permission = Builders.Permission()
               .WithContributorPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("default")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Import())
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(new ImportProductRequest()
                {
                    Content = "{\"Name\":\"default\",\"Description\":\"this is default product\",\"Features\":[{\"Name\":\"MatchScore\",\"Description\":\"show the match score on products\",\"Archived\":false,\"FeatureTags\":[],\"FeatureStates\":[],\"Toggles\":[{\"Type\":\"Esquio.Toggles.FromToToggle,Esquio\",\"Parameters\":[{\"Name\":\"From\",\"Value\":\"2020-07-13 09:36:23\",\"DeploymentName\":\"Tests\"},{\"Name\":\"To\",\"Value\":\"2020-07-29 09:36:25\",\"DeploymentName\":\"Tests\"}]}]}],\"Deployments\":[{\"Name\":\"Tests\",\"ByDefault\":true},{\"Name\":\"production\",\"ByDefault\":false}]}"
                });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task importproduct_response_forbiden_if_user_is_not_authorized()
        {
            var permission = Builders.Permission()
               .WithReaderPermission()
               .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("default")
                .Build();

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Import())
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PostAsJsonAsync(new ImportProductRequest()
                {
                    Content = "{\"Id\":1,\"Name\":\"default\",\"Description\":\"this is default product\",\"Features\":[{\"Id\":1,\"Name\":\"MatchScore\",\"Description\":\"show the match score on products\",\"ProductEntityId\":1,\"Archived\":false,\"FeatureTags\":[],\"FeatureStates\":[],\"Toggles\":[{\"Id\":1,\"Type\":\"Esquio.Toggles.FromToToggle,Esquio\",\"FeatureEntityId\":1,\"Parameters\":[{\"Id\":1,\"ToggleEntityId\":1,\"Name\":\"From\",\"Value\":\"2020-07-13 09:36:23\",\"DeploymentName\":\"Tests\"},{\"Id\":2,\"ToggleEntityId\":1,\"Name\":\"To\",\"Value\":\"2020-07-29 09:36:25\",\"DeploymentName\":\"Tests\"}]}]}],\"Deployments\":[{\"Id\":1,\"Name\":\"Tests\",\"ByDefault\":true,\"ProductEntityId\":1},{\"Id\":2,\"Name\":\"production\",\"ByDefault\":false,\"ProductEntityId\":1}]}"
                });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task exportproduct_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Export("productname"))
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task exportproduct_response_badrequest_when_product_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Export("productname"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task exportproduct_response_ok_when_success()
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

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.Export("fooproduct"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
        }


        [Fact]
        public async Task deletedeployment_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.DeleteDeployment("productname", "deploymentName"))
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletedeployment_response_forbiden_when_user_is_authenticated_but_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.DeleteDeployment("productname", "deploymentName"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletedeployment_response_badrequest_if_product_does_not_exist()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.DeleteDeployment("fooproduct", "barring"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletedeployment_response_badrequest_if_ring_does_not_exist()
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
                .CreateRequest(ApiDefinitions.V5.Product.DeleteDeployment(product.Name, "barring"))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task deletedeployment_response_badrequest_if_ring_is_default()
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
                .CreateRequest(ApiDefinitions.V5.Product.DeleteDeployment(product.Name, EsquioConstants.DEFAULT_DEPLOYMENT_NAME))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        [ResetDatabase]
        public async Task deletedeployment_response_noconent_when_success()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var product = Builders.Product()
                .WithName("fooproduct")
                .Build();

            var deployment = Builders.Deployment()
                .WithName("production")
                .Build();

            product.Deployments
                .Add(deployment);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                .CreateRequest(ApiDefinitions.V5.Product.DeleteDeployment(product.Name, deployment.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task adddeployment_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.AddDeployment("productname"))
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        [ResetDatabase]
        public async Task adddeployment_response_forbiden_when_user_is_authenticated_but_not_authorized()
        {
            var permission = Builders.Permission()
                .WithReaderPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.AddDeployment("productname"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [ResetDatabase]
        public async Task adddeployment_response_badrequest_if_ring_name_is_not_valid()
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
                  .CreateRequest(ApiDefinitions.V5.Product.AddDeployment(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddDeploymentRequest()
                  {
                      Name = "@#~invalidring@#~"
                  });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task adddeployment_response_badrequest_if_ring_name_lower_than_5_characters()
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
                  .CreateRequest(ApiDefinitions.V5.Product.AddDeployment(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddDeploymentRequest()
                  {
                      Name = "some"
                  });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task adddeployment_response_badrequest_if_ring_name_grether_than_200_characters()
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
                  .CreateRequest(ApiDefinitions.V5.Product.AddDeployment(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddDeploymentRequest()
                  {
                      Name = new string('c', 201)
                  });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task adddeployment_response_created_when_success()
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
                  .CreateRequest(ApiDefinitions.V5.Product.AddDeployment(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(new AddDeploymentRequest()
                  {
                      Name = "Production"
                  });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);
        }


        [Fact]
        public async Task list_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.List())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
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
                  .CreateRequest(ApiDefinitions.V5.Product.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

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
                  .CreateRequest(ApiDefinitions.V5.Product.List(pageIndex: 1, pageCount: 1))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

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
                  .CreateRequest(ApiDefinitions.V5.Product.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

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
                  .CreateRequest(ApiDefinitions.V5.Product.List())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
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
                  .CreateRequest(ApiDefinitions.V5.Product.List(pageIndex: 10, pageCount: 10))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .PostAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_default_deployment_name_length_is_greater_than_200()
        {
            var permission = Builders.Permission()
            .WithManagementPermission()
            .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct",
                Description = "some description",
                DefaultDeploymentName = new string('d', 201)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        [ResetDatabase]
        public async Task add_response_badrequest_if_default_deployment_name_length_is_lower_than_5()
        {
            var permission = Builders.Permission()
            .WithManagementPermission()
            .Build();

            await _fixture.Given
                .AddPermission(permission);

            var productRequest = new AddProductRequest()
            {
                Name = "fooproduct",
                Description = "some description",
                DefaultDeploymentName = new string('d', 4)
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                Description = "some description",
                DefaultDeploymentName = "Tests"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                DefaultDeploymentName = "X"
            };

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Add())
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PostAsJsonAsync(productRequest);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task get_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Get("product"))
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Get("product"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Get(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

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
                  .CreateRequest(ApiDefinitions.V5.Product.Get("product"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task delete_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Delete("product"))
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Delete("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Delete(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Delete("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
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

            var deployment = Builders.Deployment()
                .WithName(EsquioConstants.DEFAULT_DEPLOYMENT_NAME)
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
                .WithRingName(deployment.Name)
                .Build();

            toggle.Parameters
                .Add(parameter);

            feature.Toggles
                .Add(toggle);

            product.Features
                .Add(feature);

            product.Deployments
                .Add(deployment);

            await _fixture.Given
                .AddProduct(product);

            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Delete(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .DeleteAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task update_response_unauthorizaed_when_user_is_not_authenticated()
        {
            var response = await _fixture.TestServer
                  .CreateRequest(ApiDefinitions.V5.Product.Update("fooproduct"))
                  .PutAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Update("barproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Update("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Update("fooproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Update("barproduct"))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
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
                  .CreateRequest(ApiDefinitions.V5.Product.Update(product.Name))
                  .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                  .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
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
                .CreateRequest(ApiDefinitions.V5.Product.Update(product.Name))
                .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                .PutAsJsonAsync(request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
        }
    }
}
