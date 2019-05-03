using Esquio.Toggles;

namespace Esquio.Abstractions
{
    public static class IToggleExtensions
    {
        public static bool IsUserPreview(this IToggle toggle)
        {
            return toggle.GetType() == typeof(UserPreviewToggle);
        }
    }
}
