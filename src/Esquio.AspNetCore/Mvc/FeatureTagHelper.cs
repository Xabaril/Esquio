using Esquio.Abstractions;
using Esquio.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.AspNetCore.Mvc
{
    /// <summary>
    /// Razor <see cref="ITagHelper"/> for enable or disable  content depending on a feature evaluation result.
    /// </summary>
    /// <code>
    /// <![CDATA[<feature names="SomeFeature"><p>This content appair when feature 'SomeFeature' is active</p></feature>]]>
    /// </code>
    public class FeatureTagHelper
        : TagHelper
    {
        private const string FEATURE_NAME_ATTRIBUTE = "names";
        private const string INCLUDE_NAME_ATTRIBUTE = "include";
        private const string EXCLUDE_NAME_ATTRIBUTE = "exclude";
        private const string PRODUCT_NAME_ATTRIBUTE = "product";

        private static readonly char[] FeatureSeparator = new[] { ',' };

        private readonly IFeatureService _featuresService;
        private readonly ILogger<FeatureTagHelper> _logger;

        /// <summary>
        /// A coma separated list of features names to be evaluated.If any feature is not active 
        /// this tag helper suppress  the content.
        /// </summary>
        /// <remarks>
        /// The feature name are compared case insensitively with the name on the store.
        /// </remarks>
        [HtmlAttributeName(FEATURE_NAME_ATTRIBUTE)]
        public string Names { get; set; }

        /// <summary>
        /// A coma separated list of features names to be evaluated. If any feature is not active 
        /// this tag helper suppress  the content.
        /// </summary>
        /// <remarks>
        /// The feature name are compared case insensitively with the name on the store.
        /// </remarks>
        [HtmlAttributeName(INCLUDE_NAME_ATTRIBUTE)]
        public string Include { get; set; }

        /// <summary>
        /// A coma separated list of features names to be evaluated. If any feature is active
        /// this tag helper suppress the content.
        /// </summary>
        /// <remarks>
        /// The feature name are compared case insensitively with the name on the store.
        /// </remarks>
        [HtmlAttributeName(EXCLUDE_NAME_ATTRIBUTE)]
        public string Exclude { get; set; }

        /// <summary>
        /// The product name when the features are configured. If null a default product is used.
        /// </summary>
        [HtmlAttributeName(PRODUCT_NAME_ATTRIBUTE)]
        public string Product { get; set; }

        /// <inheritdoc/>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public FeatureTagHelper(IFeatureService featuresService, ILogger<FeatureTagHelper> logger)
        {
            _featuresService = featuresService ?? throw new ArgumentNullException(nameof(featuresService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Log.FeatureTagHelperBegin(_logger, Names, Product);

            var cancellationToken = ViewContext?
                .HttpContext?
                .RequestAborted ?? CancellationToken.None;

            output.TagName = null; //remove <feature> tag from the output

            if (String.IsNullOrWhiteSpace(Names)
                &&
                String.IsNullOrWhiteSpace(Include)
                &&
                String.IsNullOrWhiteSpace(Exclude))
            {
                return;
            }

            bool featureActive;

            if (Exclude != null)
            {
                var excludeFeatures = new StringTokenizer(Exclude, FeatureSeparator);

                foreach (var item in excludeFeatures)
                {
                    var featureName = item.Trim();

                    if (featureName.HasValue && featureName.Length > 0)
                    {
                        featureActive = await _featuresService.IsEnabledAsync(featureName.Value, Product, cancellationToken);

                        if (featureActive)
                        {
                            Log.FeatureTagHelperClearContent(_logger, Names, Product);

                            output.SuppressOutput();
                            return;
                        }
                    }

                }
            }

            if (Include != null)
            {
                var includeFeatures = new StringTokenizer(Include, FeatureSeparator);

                foreach (var item in includeFeatures)
                {
                    var featureName = item.Trim();

                    if (featureName.HasValue && featureName.Length > 0)
                    {
                        featureActive = await _featuresService.IsEnabledAsync(featureName.Value, Product, cancellationToken);

                        if (!featureActive)
                        {
                            Log.FeatureTagHelperClearContent(_logger, Names, Product);

                            output.SuppressOutput();
                            return;
                        }
                    }

                }
            }

            if (Names != null)
            {
                var namedFeatures = new StringTokenizer(Names, FeatureSeparator);

                foreach (var item in namedFeatures)
                {
                    var featureName = item.Trim();

                    if (featureName.HasValue && featureName.Length > 0)
                    {
                        featureActive = await _featuresService.IsEnabledAsync(featureName.Value, Product, cancellationToken);

                        if (!featureActive)
                        {
                            Log.FeatureTagHelperClearContent(_logger, Names, Product);

                            output.SuppressOutput();
                            return;
                        }
                    }
                }
            }
        }
    }
}
