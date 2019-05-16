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
        public void not_allow_to_add_null_toggles()
        {
            var feature = new Feature("test");

            Action act = () => feature.AddToggles(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
