namespace Esquio.Configuration.Store.Entities
{
    public class Toggle
    {
        public string Type { get; set; }
        public Parameter[] Parameters { get; set; } = new Parameter[0];
    }
}
