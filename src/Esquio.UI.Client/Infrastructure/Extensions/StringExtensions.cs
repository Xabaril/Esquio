namespace System
{
    public static class StringExtensions
    {
        public static bool HasValue(this string @string) => !string.IsNullOrWhiteSpace(@string);
    }
}
