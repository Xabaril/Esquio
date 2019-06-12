using Esquio.Abstractions;
using Esquio.Diagnostics;
using Esquio.Toggles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Esquio
{
    internal sealed class DefaultToggleTypeActivator
        : IToggleTypeActivator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DefaultToggleTypeActivator> _logger;

        public DefaultToggleTypeActivator(IServiceProvider serviceProvider, ILogger<DefaultToggleTypeActivator> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IToggle CreateInstance(string toggleTypeName)
        {
            Log.DefaultToggleTypeActivatorResolveTypeBegin(_logger, toggleTypeName);

            if (_typesCache.TryGetValue(toggleTypeName, out Type type))
            {
                Log.DefaultToggleTypeActivatorTypeIsResolved(_logger, toggleTypeName);
                return (IToggle)_serviceProvider.GetService(type);
            }
            else
            {
                var toggleType = Type.GetType(toggleTypeName, throwOnError: false, ignoreCase: true);

                if (toggleType != null)
                {
                    _typesCache.TryAdd(toggleTypeName, toggleType);

                    Log.DefaultToggleTypeActivatorTypeIsResolved(_logger, toggleTypeName);
                    return (IToggle)_serviceProvider.GetService(toggleType);
                }
            }

            Log.DefaultToggleTypeActivatorTypeCantResolved(_logger, toggleTypeName);
            return null;
        }

        private ConcurrentDictionary<string, Type> _typesCache = new ConcurrentDictionary<string, Type>(new List<KeyValuePair<string, Type>>()
        {
            new KeyValuePair<string, Type>(typeof(OnToggle).FullName,typeof(OnToggle)),
            new KeyValuePair<string, Type>(typeof(OffToggle).FullName,typeof(OffToggle)),
            new KeyValuePair<string, Type>(typeof(FromToToggle).FullName,typeof(FromToToggle)),
            new KeyValuePair<string, Type>(typeof(EnvironmentToggle).FullName,typeof(EnvironmentToggle)),
            new KeyValuePair<string, Type>(typeof(RoleNameToggle).FullName,typeof(RoleNameToggle)),
            new KeyValuePair<string, Type>(typeof(UserNameToggle).FullName,typeof(UserNameToggle)),
            new KeyValuePair<string, Type>(typeof(GradualRolloutUserNameToggle).FullName,typeof(GradualRolloutUserNameToggle))
        });
    }
}
