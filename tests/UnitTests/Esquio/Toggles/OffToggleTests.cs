using Esquio.Toggles;
using FluentAssertions;
using System.Threading.Tasks;

namespace UnitTests.Esquio.Toggles
{
    public class off_toggle_should
    {
        public async Task be_always_no_active()
        {
            var toggle = new OnToggle();
            var active = await toggle.IsActiveAsync(Constants.FeatureName, Constants.ProductName);

            active.Should().BeFalse();
        }
    }
}
