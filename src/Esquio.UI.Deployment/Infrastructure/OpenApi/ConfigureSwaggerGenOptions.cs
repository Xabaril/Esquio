using Esquio.UI.Api.Infrastructure.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Esquio.UI.Deployment.Infrastructure.OpenApi
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersion;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ConfigureSwaggerGenOptions(
            IConfiguration configuration,
            IApiVersionDescriptionProvider apiVersion,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public void Configure(SwaggerGenOptions options)
        {
            var securitySettings = new SecuritySettings();
            _configuration.Bind("Security", securitySettings);

            var discoveryDocument = GetDiscoveryDocument(securitySettings);

            options.OperationFilter<AuthorizeOperationFilter>();

            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(x => x.FullName);

            foreach (var description in _apiVersion.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfoForApiVersion(description));
            }

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows {
                    AuthorizationCode = new OpenApiOAuthFlow {
                        AuthorizationUrl = new Uri(discoveryDocument.AuthorizeEndpoint),
                        TokenUrl = new Uri(discoveryDocument.TokenEndpoint),
                        Scopes = new Dictionary<string, string>
                        {
                            { securitySettings.OpenId.Scope , "Esquio UI HTTP API" }
                        },
                    }
                },
                Description = "Esquio UI OpenId Security Scheme"
            });

            options.AddSecurityDefinition("apikey", new OpenApiSecurityScheme {
                Type = SecuritySchemeType.ApiKey,
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Description = "Esquio UI Api Key Security Scheme"
            });
        }

        private static OpenApiInfo CreateOpenApiInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo() {
                Title = "Esquio UI API",
                Version = description.ApiVersion.ToString(),
                Description = "Esquio UI API",
                Contact = new OpenApiContact() { Name = "Xabaril", Url = new Uri("https://github.com/Xabaril") },
                License = new OpenApiLicense()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        private DiscoveryDocumentResponse GetDiscoveryDocument(SecuritySettings settings)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var authority = settings.OpenId.Authority;

            if (settings.IsAzureAd)
            {
                authority = $"{settings.OpenId.Authority}/v2.0";
            }

            var request = new DiscoveryDocumentRequest() {
                Address = authority,
                Policy = new DiscoveryPolicy() {
                    ValidateEndpoints = settings.IsAzureAd ? false : true
                }
            };
            var discoveryDocument = httpClient.GetDiscoveryDocumentAsync(request)
                .GetAwaiter()
                .GetResult();

            return discoveryDocument;
        }

    }
}
