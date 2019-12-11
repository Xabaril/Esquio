using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace Esquio.UI.Infrastructure.OpenApi
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersion;
        private readonly IConfiguration _configuration;

        public ConfigureSwaggerGenOptions(IConfiguration configuration, IApiVersionDescriptionProvider apiVersion)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<AuthorizeOperationFilter>();

            options.DescribeAllParametersInCamelCase();
            options.CustomSchemaIds(x => x.FullName);

            foreach (var description in _apiVersion.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfoForApiVersion(description));
            }

            //add security definitions swagger
            // we added implicit ( legacy ) because swagger-ui does not support code+pkce at this momment
            // we send a PR to solve it  but at this momment is not merged!!

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{_configuration["Security:Jwt:Authority"]}/connect/authorize"),
                        Scopes = new Dictionary<string, string>
                        {
                            {_configuration["Security:Jwt:Audience"] , "Esquio UI HTTP Api"}
                        }
                    }
                },
                Description = "Esquio UI OpenId Security Scheme"
            });

            options.AddSecurityDefinition("apikey", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Description = "Esquio UI Api Key Security Scheme"
            });
        }

        private static OpenApiInfo CreateOpenApiInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
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
    }
}
