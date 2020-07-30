using Esquio.UI.Api.Diagnostics;
using Esquio.UI.Api.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace Esquio.UI.Api.Infrastructure.Services
{
    public class DefaultSubjectClaimsPrincipalFactory
        :ISubjectClaimsPrincipalFactory
    {
        private readonly SecuritySettings _settings;
        private readonly ILogger<DefaultSubjectClaimsPrincipalFactory> _logger;

        public DefaultSubjectClaimsPrincipalFactory(IOptions<SecuritySettings> options, ILogger<DefaultSubjectClaimsPrincipalFactory> logger)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetSubject(ClaimsPrincipal principal)
        {
            var defaultSubjectClaimType = _settings.DefaultSubjectClaimType ?? ApiConstants.SubjectNameIdentifier;

            var subject =  principal
                .FindFirstValue(defaultSubjectClaimType);

            if (String.IsNullOrEmpty(subject))
            {
                Log.DefaultSubjectIsNull(_logger);
            }

            return subject;
        }
    }
}
