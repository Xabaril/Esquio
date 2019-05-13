using Esquio;
using Esquio.Model;
using Esquio.Toggles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Esquio
{
    public class feature_should
    {
        [Fact]
        public void not_allow_create_with_an_invalid_name()
        {
            Action act = () => new Feature(name: null, description: "description");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_allow_to_add_null_toggle()
        {
            var feature = new Feature("test", "test");

            Action act = () => feature.AddToggle(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_allow_to_add_null_toggles()
        {
            var feature = new Feature("test", "test");

            Action act = () => feature.AddToggles(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_allow_to_add_more_than_one_toggle_when_is_marked_as_preview()
        {
            var feature = new Feature("test", "test");
            feature.MarkAsPreview();
            var toggles = new List<Toggle>
            {
                new Toggle(type: typeof(OnToggle).FullName),
                new Toggle(type: typeof(UserNameToggle).FullName)
            };

            Action act = () => feature.AddToggles(toggles);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void not_allow_to_add_a_toogle_that_is_not_of_the_type_userpreviewtoggle_when_is_marked_as_preview()
        {
            var feature = new Feature("test", "test");
            feature.MarkAsPreview();
            var toggles = new List<Toggle>
            {
                new Toggle(type: typeof(UserNameToggle).FullName)
            };

            Action act = () => feature.AddToggles(toggles);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void allow_to_add_a_toogle_that_is_of_the_type_userpreviewtoggle_when_is_marked_as_preview()
        {
            var feature = new Feature("test", "test");
            var toggles = new List<Toggle>
            {
                new Toggle(type: typeof(UserPreviewToggle).FullName)
            };
            feature.MarkAsPreview();

            Action act = () => feature.AddToggles(toggles);

            act.Should().NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void not_allow_to_marked_as_preview_if_feature_has_more_than_one_toggle()
        {
            var feature = new Feature("test", "test");
            var toggles = new List<Toggle>
            {
                new Toggle(type: typeof(OnToggle).FullName),
                new Toggle(type: typeof(OffToggle).FullName),
            };
            
            feature.AddToggles(toggles);
            Action act = () => feature.MarkAsPreview();

            act.Should().Throw<EsquioException>();
        }

        [Fact]
        public void not_allow_to_marked_as_preview_if_feature_has_one_toggle_and_is_not_a_preview_toggle()
        {
            var feature = new Feature("test", "test");
            var toggles = new List<Toggle>
            {
                new Toggle(type: typeof(OnToggle).FullName),
            };

            feature.AddToggles(toggles);
            Action act = () => feature.MarkAsPreview();

            act.Should().Throw<EsquioException>();
        }
    }
}
