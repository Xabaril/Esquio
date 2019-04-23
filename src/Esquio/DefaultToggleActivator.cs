using Esquio.Abstractions;
using Esquio.Diagnostics;
using Microsoft.Extensions.Logging;
using System;

namespace Esquio
{
    public class DefaultToggleTypeActivator
        : IToggleTypeActivator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DefaultToggleTypeActivator> _logger;

        public DefaultToggleTypeActivator(IServiceProvider serviceProvider,ILogger<DefaultToggleTypeActivator> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IToggle CreateInstance(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorResolveTypeBegin(_logger, toggleTypeName);

            var type = Type.GetType(toggleTypeName);
            return (IToggle)_serviceProvider.GetService(type);
        }
    }
}
