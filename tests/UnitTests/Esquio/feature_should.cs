using Esquio;
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
            Action act = () => new Feature(name: null, description: "description");
            act.Should().Throw<ArgumentException>().WithMessage("name");
        }

        [Fact]
        public void not_allow_create_with_an_invalid_descrption()
        {
            Action act = () => new Feature(name: "name", description: null);
            act.Should().Throw<ArgumentException>().WithMessage("description");
        }
    }
}
