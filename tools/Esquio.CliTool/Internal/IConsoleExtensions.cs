using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;

namespace Esquio.CliTool.Internal
{
    public static class IConsoleExtensions
    {
        public static void WriteObject<TDetails>(this IConsole console, TDetails details, ConsoleColor color) where TDetails : class
        {
            var defaultForegroundColor = console.ForegroundColor;
            console.ForegroundColor = color;
            console.WriteLine(JsonConvert.SerializeObject(details, Formatting.Indented));
            console.ForegroundColor = defaultForegroundColor;
        }

        public static void WriteLine(this IConsole console, string message, ConsoleColor color)
        {
            var defaultForegroundColor = console.ForegroundColor;
            console.ForegroundColor = color;
            console.WriteLine(message);
            console.ForegroundColor = defaultForegroundColor;
        }
    }
}
