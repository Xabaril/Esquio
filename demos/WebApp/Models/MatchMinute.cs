namespace WebApp.Models
{
    public class MatchMinute
    {
        public int Minute { get; set; }
        public MatchEvent Event { get; set; } = MatchEvent.Default;
        public string Text { get; set; }
    }
}
