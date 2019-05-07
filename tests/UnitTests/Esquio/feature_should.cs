using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Esquio.Model;
using Esquio.Toggles;
using FluentAssertions;
using NSubstitute;
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

            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void not_allow_to_add_null_toggles()
        {
            var feature = new Feature("test", "test");

            Action act = () => feature.AddToggles(null);

            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void not_allow_to_add_more_than_one_toggle_when_is_marked_as_preview()
        {
            var feature = new Feature("test", "test", isPreview: true);
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
            var feature = new Feature("test", "test", isPreview: true);
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
            var feature = new Feature("test", "test", isPreview: true);
            var toggles = new List<Toggle>
            {
                new Toggle(type: typeof(UserPreviewToggle).FullName)
            };

            Action act = () => feature.AddToggles(toggles);

            act.Should().NotThrow<InvalidOperationException>();
        }
    }
}
