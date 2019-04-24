using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// Use this attribute to filter if an MVC action can be executed depending on the configured
    /// feature activation state. If the configured feature is active this action can be executed, if not
    /// by default a NotFound result is the returned action result. You can modify the default action using
    /// the extension method AddMvcFallbackAction in <see cref="IEsquioBuilder"/> interface when register Esquio services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class Flag
        : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        public string FeatureName { get; set; }
        public string ApplicationName { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            return new FlagFilter(scopeFactory, FeatureName, ApplicationName);
        }
    }
    internal class FlagFilter
        : IAsyncResourceFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _featureName;
        private readonly string _applicationName;

        public FlagFilter(IServiceScopeFactory serviceScopeFactory, string featureName, string applicationName)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _featureName = featureName;
            _applicationName = applicationName;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider
                    .GetRequiredService<ILogger<Flag>>();
                var featureService = scope.ServiceProvider
                    .GetRequiredService<IFeatureService>();
                var fallbackService = scope.ServiceProvider
                    .GetService<IMvcFallbackService>();

                Log.FlagBegin(logger, _featureName, _applicationName);

                if (await featureService.IsEnabledAsync(_featureName, _applicationName))
                {
                    Log.FlagExecutingAction(logger, _featureName, _applicationName);

                    await next();
                }
                else
                {
                    Log.FlagNonExecuteAction(logger, _featureName, _applicationName);

                    if (fallbackService != null)
                    {
                        context.Result = fallbackService.GetFallbackActionResult(context);
                    }
                    else
                    {
                        Log.FlagFallbackServiceIsNotConfigured(logger);

                        context.Result = new NotFoundResult()
                        {

                        };
                    }
                }
            }
        }
    }
}
