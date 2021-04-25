using System;

namespace Esquio.UI.Host.Infrastructure.Configuration
{
    public class EnvironmentVariable
    {
        public static string GetValue(string variable) =>
            Environment.GetEnvironmentVariable(variable);

        public static bool HasValue(string variable) =>
            !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable));
    }
}
