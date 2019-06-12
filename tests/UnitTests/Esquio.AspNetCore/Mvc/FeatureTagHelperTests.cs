using Esquio.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Seedwork;
using Xunit;

namespace UnitTests.Esquio.AspNetCore.Mvc
{
    public class featuretaghelper_should
    {
        [Fact]
        public async Task clean_content_when_feature_disabled()
        {
            var featureService = new DelegatedFeatureService((_, __) => false);
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Names = "Feature-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.IsEmptyOrWhiteSpace
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task clean_content_when_feature_is_enabled_and_is_on_exclude_attribute()
        {
            var featureService = new DelegatedFeatureService((_, __) => true);
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Exclude = "Feature-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.IsEmptyOrWhiteSpace
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task preserve_content_when_feature_is_disabled_and_is_on_exclude_attribute()
        {
            var featureService = new DelegatedFeatureService((_, __) => false);
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Exclude = "Feature-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.IsEmptyOrWhiteSpace
                .Should()
                .BeFalse();
        }


        [Fact]
        public async Task preserve_content_when_feature_is_enabled_and_is_on_include_attribute()
        {
            var featureService = new DelegatedFeatureService((_, __) => true);
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Include = "Feature-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.IsEmptyOrWhiteSpace
                .Should()
                .BeFalse();
        }
        [Fact]
        public async Task clean_content_when_feature_is_disabled_and_is_on_include_attribute()
        {
            var featureService = new DelegatedFeatureService((_, __) => false);
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Include = "Feature-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.IsEmptyOrWhiteSpace
                .Should()
                .BeTrue();
        }

        [Fact]
        public async Task preserve_content_when_feature_is_enabled()
        {
            var featureService = new DelegatedFeatureService((_, __) => true);
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Names = "Feature-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.GetContent().Should().Contain("some content");
        }

        [Fact]
        public async Task preserve_content_when_feature_is_enabled_for_specified_product()
        {
            var featureService = new DelegatedFeatureService((_, _product) =>
            {
                return _product.Equals("name-product-1");
            });
            var logger = new LoggerFactory().CreateLogger<FeatureTagHelper>();

            var tagHelper = new FeatureTagHelper(featureService, logger)
            {
                Names = "Feature-1",
                Product = "name-product-1"
            };

            var context = CreateTagHelperContext();
            var output = CreateTagHelperOutput(tag: "feature", innerContent: "<p>some content</p>");

            await tagHelper.ProcessAsync(context, output);

            output.Content.GetContent().Should().Contain("some content");
        }
        private TagHelperContext CreateTagHelperContext()
        {
            return new TagHelperContext(
                new TagHelperAttributeList(Enumerable.Empty<TagHelperAttribute>()),
                new Dictionary<object, object>(),
                "test");
        }
        private TagHelperOutput CreateTagHelperOutput(string tag, string innerContent)
        {
            var output = new TagHelperOutput(tag,
               new TagHelperAttributeList(),
               (useCachedResult_, encoder) =>
               {
                   var tagHelperContent = new DefaultTagHelperContent();

                   tagHelperContent.SetContent(innerContent);

                   return Task.FromResult<TagHelperContent>(tagHelperContent);
               });

            output.PreContent.SetContent("precontent");
            output.Content.SetContent(innerContent);
            output.PostContent.SetContent("postcontent");

            return output;
        }
    }
}
