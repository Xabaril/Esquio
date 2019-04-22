namespace Esquio.Configuration.Store.Configuration
{
    internal class ToggleConfiguration
    {
        public string Type { get; set; }
        public ParameterConfiguration[] Parameters { get; set; } = new ParameterConfiguration[0];
    }
}
