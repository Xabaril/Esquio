namespace Esquio.UI.Api.Shared.Settings
{
    public class BlazorClientSettings
    {
        public ClientOpenIdSecurity Security { get; set; } = new ClientOpenIdSecurity();
    }

    public class ClientOpenIdSecurity
    {
        public bool IsAzureAd { get; set; }

        public string ClientId { get; set; }

        public string Authority { get; set; }

        public string Scope { get; set; }

        public string Audience { get; set; }

        public string ResponseType { get; set; }
    }
}
