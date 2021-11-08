using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Deployment.Infrastructure.OpenApi
{
    class AuthorizeOperationFilter
        : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
#pragma warning disable CS0618

            var hasAuthorize = context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any() ||
                               context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any();
#pragma warning restore 

            if (hasAuthorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();

                var oauth2SecurityScheme = new OpenApiSecurityScheme() {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                };

                var apiKeySecurityScheme = new OpenApiSecurityScheme() {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apikey" },
                };

                operation.Security.Add(new OpenApiSecurityRequirement() {
                    [oauth2SecurityScheme] = new[] { "oauth2" },
                    [apiKeySecurityScheme] = new[] { "apikey" }
                });
            }
        }
    }
}
