using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
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
    public class FeatureTagHelper
        : TagHelper
    {
        private const string FEATURE_NAME_ATTRIBUTE = "featureName";
        private const string INCLUDE_NAME_ATTRIBUTE = "include";
        private const string EXCLUDE_NAME_ATTRIBUTE = "exclude";
        private const string APPLICATION_NAME_ATTRIBUTE = "applicationName";

        private static readonly char[] FeatureSeparator = new[] { ',' };

        private readonly IFeatureService _featuresService;
        private readonly ILogger<FeatureTagHelper> _logger;

        [HtmlAttributeName(FEATURE_NAME_ATTRIBUTE)]
        public string FeatureName { get; set; }

        [HtmlAttributeName(INCLUDE_NAME_ATTRIBUTE)]
        public string Include { get; set; }

        [HtmlAttributeName(EXCLUDE_NAME_ATTRIBUTE)]
        public string Exclude { get; set; }

        [HtmlAttributeName(APPLICATION_NAME_ATTRIBUTE)]
        public string ApplicationName { get; set; }

        public FeatureTagHelper(IFeatureService featuresService, ILogger<FeatureTagHelper> logger)
        {
            _featuresService = featuresService ?? throw new ArgumentNullException(nameof(featuresService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Log.FeatureTagHelperBegin(_logger, FeatureName, ApplicationName);

            output.TagName = null; //remove <feature> tag from the output

            if (String.IsNullOrWhiteSpace(FeatureName)
                &&
                String.IsNullOrWhiteSpace(Include)
                &&
                String.IsNullOrWhiteSpace(Exclude))
            {
                return;
            }

            bool featureActive = false;

            if (Exclude != null)
            {
                var excludeFeatures = new StringTokenizer(Exclude, FeatureSeparator);

                foreach (var item in excludeFeatures)
                {
                    featureActive = await _featuresService
                        .IsEnabledAsync(featureName: item.Trim().Value, ApplicationName);

                    if (featureActive)
                    {
                        Log.FeatureTagHelperClearContent(_logger, FeatureName, ApplicationName);

                        output.SuppressOutput();
                        return;
                    }
                }
            }

            if (Include != null)
            {
                var includeFeatures = new StringTokenizer(Include, FeatureSeparator);

                foreach (var item in includeFeatures)
                {
                    featureActive = await _featuresService
                        .IsEnabledAsync(item.Trim().Value, ApplicationName);

                    if (!featureActive)
                    {
                        Log.FeatureTagHelperClearContent(_logger, FeatureName, ApplicationName);

                        output.SuppressOutput();
                        return;
                    }
                }
            }

            if (FeatureName != null)
            {
                featureActive = await _featuresService
                  .IsEnabledAsync(FeatureName, ApplicationName);

                if (!featureActive)
                {
                    Log.FeatureTagHelperClearContent(_logger, FeatureName, ApplicationName);

                    output.SuppressOutput();
                    return;
                }
            }
        }
    }
}
