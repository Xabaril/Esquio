namespace Esquio.Configuration.Store.Entities
{
    public class Application
    {
        public string Name { get; set; }
        public Feature[] Features { get; set; } = new Feature[0];
    }
}
