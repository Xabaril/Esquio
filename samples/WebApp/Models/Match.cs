using System.Collections.Generic;

namespace WebApp.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string Visitor { get; set; }
        public string Time { get; set; }
        public MatchState State { get; set; } = MatchState.Pending;
        public string Description { get; set; }
        public int ScoreLocal { get; set; } = 0;
        public int ScoreVisitor { get; set; } = 0;
        public IEnumerable<MatchMinute> Minutes { get; set; } 
    }
}
