using Esquio;
using FluentAssertions;

namespace UnitTests.Esquio
{
    public class FeatureTests
    {
        public void CreateEnabled_create_enabled_toogle()
        {
            var toogle = Feature.CreateEnabled("toogle#1");

            toogle.Should().NotBeNull();
            toogle.Name.Should().BeEquivalentTo("toogle#1");
            toogle.Enabled.Should().BeTrue();
        }
        public void CreateDisabled_create_disabled_toogle()
        {
            var toogle = Feature.CreateEnabled("toogle#2");

            toogle.Should().NotBeNull();
            toogle.Name.Should().BeEquivalentTo("toogle#2");
            toogle.Enabled.Should().BeFalse();
        }
    }
}
