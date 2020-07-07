using Esquio.UI.Api;
using Esquio.UI.Api.Infrastructure.Data.DbContexts;
using Esquio.UI.Api.Infrastructure.Data.Options;
using Esquio.UI.Store.Infrastructure.Data;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            return services
                .AddProblemDetails(setup =>
                {
                    setup.Map<InvalidOperationException>(exception =>
                    {
                        return new ProblemDetails()
                        {
                            Detail = exception.Message,
                            Status = StatusCodes.Status400BadRequest
                        };
                    });
                })
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
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, Action<StoreOptions> setup)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = setup ?? throw new ArgumentNullException(nameof(setup));

            services.Configure(setup);

            return services.AddEntityFramework(configuration, environment);
        }

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            switch (configuration["Data:Store"])
            {
                case "NpgSql":
                    {
                        return services.AddEntityFrameworkNpgSql(configuration, environment);
                    }
                case "SqlServer":
                    {
                        return services.AddEntityFrameworkSqlServer(configuration, environment);
                    }

                default: throw new InvalidOperationException("Add EntityFramework requires either Data:Store:SqlServer or Data:Store:Npgsql to be set.");
            }

        }
        public static IServiceCollection AddEntityFrameworkSqlServer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration["Data:ConnectionString"];
            return services.AddDbContext<StoreDbContext>(builder => builder.SetupSqlServer(connectionString).SetupSensitiveLogging(environment));
        }
        public static IServiceCollection AddEntityFrameworkNpgSql(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connectionString = configuration["Data:ConnectionString"];
            services.Configure<StoreOptions>(setup => setup.DefaultSchema = "public");
            return services.AddDbContext<StoreDbContext, NpgSqlContext>(builder => builder.SetupNpgSql(connectionString).SetupSensitiveLogging(environment));
        }
        public static DbContextOptionsBuilder SetupSensitiveLogging(this DbContextOptionsBuilder optionsBuilder, IWebHostEnvironment environment)
        {
            return optionsBuilder.EnableDetailedErrors()
                    .EnableSensitiveDataLogging(environment.IsDevelopment());
        }
        public static DbContextOptionsBuilder SetupSqlServer(this DbContextOptionsBuilder contextOptionsBuilder, string connectionString)
        {
            return contextOptionsBuilder.UseSqlServer(connectionString, setup =>
            {
                setup.MaxBatchSize(10);
                setup.EnableRetryOnFailure();
                setup.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
            });
        }
        public static DbContextOptionsBuilder SetupNpgSql(this DbContextOptionsBuilder contextOptionsBuilder, string connectionString)
        {
            return contextOptionsBuilder.UseNpgsql(connectionString, setup =>
            {
                setup.MaxBatchSize(10);
                setup.EnableRetryOnFailure();
                setup.MigrationsAssembly(typeof(NpgSqlDesignTimeContextFactory).Assembly.FullName);
            });
        }
    }
}
