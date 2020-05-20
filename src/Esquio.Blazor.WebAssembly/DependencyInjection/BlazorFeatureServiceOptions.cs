namespace Esquio.Blazor.WebAssembly.DependencyInjection
{
    /// <summary>
    /// Blazor feature service options.
    /// </summary>
    public class BlazorFeatureServiceOptions
    {
        const string DEFAULT_ESQUIO_ENDPOINT = "Esquio";

        /// <summary>
        /// The default endpoint to use.
        /// </summary>
        public string Endpoint { get; set; } = DEFAULT_ESQUIO_ENDPOINT;
    }
}
