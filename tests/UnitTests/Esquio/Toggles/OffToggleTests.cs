using Esquio.Toggles;
using FluentAssertions;
using System.Threading.Tasks;

namespace UnitTests.Esquio.Toggles
{
    public class OffToggleShould
    {
        public async Task be_always_no_active()
        {
            var toggle = new OnToggle();
            var active = await toggle.IsActiveAsync(null);

            active.Should().BeFalse();
        }
    }
}
