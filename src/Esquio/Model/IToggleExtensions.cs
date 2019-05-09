using Esquio.Model;
using Esquio.Toggles;
using System;

namespace Esquio.Abstractions
{
    public static class IToggleExtensions
    {
        public static bool IsUserPreview(this Toggle toggle)
        {
            return toggle.Type.Equals(typeof(UserPreviewToggle).FullName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
