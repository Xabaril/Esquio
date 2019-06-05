using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// Use this attribute to filter if an MVC action can be executed depending on features active state.
    /// If the configured feature is active this action can be executed, if not
    /// by default a NotFound result is the returned action result. You can modify the default action using
    /// the extension method AddMvcFallbackAction in <see cref="IEsquioBuilder"/> interface when register Esquio services.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class FeatureFilter
        : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        public string Names { get; set; }
        public string ProductName { get; set; }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            return new FeatureResourceFilter(scopeFactory, Names, ProductName);
        }
    }
    internal class FeatureResourceFilter
        : IAsyncResourceFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _featureNames;
        private readonly string _productName;

        public FeatureResourceFilter(IServiceScopeFactory serviceScopeFactory, string featureNames, string productName)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _featureNames = featureNames ?? throw new ArgumentNullException(nameof(featureNames));
            _productName = productName ?? string.Empty;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            const char SPLIT_CHAR = ';';

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider
                    .GetRequiredService<ILogger<FeatureFilter>>();

                var featureService = scope.ServiceProvider
                    .GetRequiredService<IFeatureService>();

                var fallbackService = scope.ServiceProvider
                    .GetService<IMvcFallbackService>();

                Log.FeatureFilterBegin(logger, _featureNames, _productName);

                var tokenizer = new StringTokenizer(_featureNames, new char[] { SPLIT_CHAR });

                foreach (var item in tokenizer)
                {
                    if (!await featureService.IsEnabledAsync(item.Value, _productName))
                    {
                        Log.FeatureFilterNonExecuteAction(logger, item.Value, _productName);
                        context.Result = fallbackService.GetFallbackActionResult(context);

                        return;
                    }
                }

                Log.FeatureFilterExecutingAction(logger, _featureNames, _productName);
                await next();
            }
        }
    }
}
