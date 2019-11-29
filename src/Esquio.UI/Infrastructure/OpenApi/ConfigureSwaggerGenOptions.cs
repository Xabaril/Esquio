using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Esquio.UI.Infrastructure.OpenApi
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersion;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersion)
        {
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.CustomSchemaIds(x => x.FullName);
            foreach (var description in _apiVersion.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfoForApiVersion(description));
            }
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
