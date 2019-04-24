using Esquio.Abstractions;
using Microsoft.AspNetCore.Razor.TagHelpers;
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

        private readonly IFeatureService _featuresService;

        [HtmlAttributeName(FEATURE_NAME_ATTRIBUTE)]
        public string FeatureName { get; set; }

        public FlagTagHelper(IFeatureService featuresService)
        {
            _featuresService = featuresService ?? throw new ArgumentNullException(nameof(featuresService));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            if (!childContent.IsEmptyOrWhiteSpace)
            {
                var isActive = await _featuresService.IsEnabledAsync(FeatureName);

                if (!isActive)
                {
                    output.Content.SetContent(string.Empty);
                }
            }
        }
    }
}
