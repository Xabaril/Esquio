using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// Razor TagHelper for enable or disable inner content depending on a feature activation state.
    /// </summary>
    /// <code>
    /// <![CDATA[<flag featureName="SomeFeature"><p>This content appair when feature 'SomeFeature' is active</p></flag>]]>
    /// </code>
    [HtmlTargetElement("flag", Attributes = FEATURE_NAME_ATTRIBUTE)]
    public class FlagTagHelper
        : TagHelper
    {
        private const string FEATURE_NAME_ATTRIBUTE = "featureName";
        private const string APPLICATION_NAME_ATTRIBUTE = "applicationName";

        private readonly IFeatureService _featuresService;
        private readonly ILogger<FlagTagHelper> _logger;

        [HtmlAttributeName(FEATURE_NAME_ATTRIBUTE)]
        public string FeatureName { get; set; }

        [HtmlAttributeName(APPLICATION_NAME_ATTRIBUTE)]
        public string ApplicationName { get; set; }

        public FlagTagHelper(IFeatureService featuresService, ILogger<FlagTagHelper> logger)
        {
            _featuresService = featuresService ?? throw new ArgumentNullException(nameof(featuresService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Log.FlagTagHelperBegin(_logger, FeatureName, ApplicationName);

            var childContent = await output.GetChildContentAsync();

            if (!childContent.IsEmptyOrWhiteSpace)
            {
                var isActive = await _featuresService.IsEnabledAsync(FeatureName, ApplicationName);

                if (!isActive)
                {
                    Log.FlagTagHelperClearContent(_logger, FeatureName, ApplicationName);

                    output.Content.SetContent(string.Empty);
                }
            }
        }
    }
}
