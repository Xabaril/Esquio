﻿using FluentAssertions;
using FunctionalTests.Esquio.UI.Api.Seedwork;
using FunctionalTests.Esquio.UI.Api.Seedwork.Builders;
using FunctionalTests.Esquio.UI.Api.Seedwork.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Esquio.UI.Api.Scenarios.Versioning
{
    [Collection(nameof(AspNetCoreServer))]
    public class api_versioning_should
    {
        private readonly ServerFixture _fixture;
        public api_versioning_should(ServerFixture serverFixture)
        {
            _fixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        [ResetDatabase]
        public async Task use_header_version_if_client_specify_booth()
        {
            var permission = Builders.Permission()
                .WithManagementPermission()
                .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V5.Permissions.My())
                 .AddHeader("X-API-VERSION", "X.X")
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        [ResetDatabase]
        public async Task use_header_version()
        {
            var permission = Builders.Permission()
              .WithManagementPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest(ApiDefinitions.V5.Permissions.My())
                 .AddHeader("X-API-VERSION", Constants.DEFAULT_HTTPAPI_VERSION)
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        [ResetDatabase]
        public async Task use_querystring_version()
        {
            var permission = Builders.Permission()
              .WithManagementPermission()
              .Build();

            await _fixture.Given
                .AddPermission(permission);

            var response = await _fixture.TestServer
                 .CreateRequest("api/permissions/my")
                 .WithIdentity(Builders.Identity().WithDefaultClaims().Build())
                 .GetAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

    }
}
