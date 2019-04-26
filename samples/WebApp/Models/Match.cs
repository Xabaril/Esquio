namespace WebApp.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string Visitor { get; set; }
        public string Time { get; set; }
        public MatchState State { get; set; }
        public string Description { get; set; }
    }
}
