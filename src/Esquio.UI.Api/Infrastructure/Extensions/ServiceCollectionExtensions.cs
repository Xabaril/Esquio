using Esquio.UI.Api;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services) =>
            services
                .AddProblemDetails()
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Instance = context.HttpContext.Request.Path,
                            Status = StatusCodes.Status400BadRequest,
                            Type = $"https://httpstatuses.com/400",
                            Detail = ApiConstants.Messages.ModelStateValidation
                        };
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes =
                            {
                                    ApiConstants.ContentTypes.ProblemJson,
                                    ApiConstants.ContentTypes.ProblemXml
                            }
                        };
                    };
                });
    }
}
