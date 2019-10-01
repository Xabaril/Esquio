using StackExchange.Profiling.Internal;
using System;

namespace MiniProfiler.Esquio.DependencyInjection
{
    /// <summary>
    /// Extension methods for the MiniProfiler.Esquio.
    /// </summary>
    public static class MiniProfilerBaseOptionsExtensions
    {
        /// <summary>
        /// Adds Esquio profiling for MiniProfiler via DiagnosticListener.
        /// </summary>
        /// <typeparam name="T">The specific options type to chain with.</typeparam>
        /// <param name="options">The <see cref="MiniProfilerBaseOptions" /> to register on (just for chaining).</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is <c>null</c>.</exception>
        public static T AddEsquio<T>(this T options) where T : MiniProfilerBaseOptions
        {
            var initializer = new DiagnosticInitializer(new[] { new EsquioDiagnosticListener() });
            initializer.Start();

            return options;
        }
    }
}
