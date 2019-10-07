using Esquio.Model;
using FluentAssertions;
using System;
using Xunit;

namespace UnitTests.Esquio
{
    public class feature_should
    {
        [Fact]
        public void not_allow_create_with_an_invalid_name()
        {
            Action act = () => new Feature(name: null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_allow_to_add_null_toggle()
        {
            var feature = new Feature("test");

            Action act = () => feature.AddToggle(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void allow_add_toggle()
        {
            var feature = new Feature("test");
            var toggle = new Toggle("toggle");

            feature.AddToggle(toggle);

            feature.GetToggle(toggle.Type).Should().BeSameAs(toggle);
        }

        [Fact]
        public void not_allow_to_add_null_toggles()
        {
            var feature = new Feature("test");

            Action act = () => feature.AddToggles(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void not_allow_to_add_null_element_toggles()
        {
            var feature = new Feature("test");

            Action act = () => feature.AddToggles(new Toggle[] { null });

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void allow_add_multiple_toggles()
        {
            var feature = new Feature("test");
            var toggles = new[] { new Toggle("toggle1"), new Toggle("toggle2") };

            feature.AddToggles(toggles);

            feature.GetToggles().Should().BeEquivalentTo(toggles);
        }
    }
}
