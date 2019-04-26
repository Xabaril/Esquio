using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Services
{
    public class MatchService : IMatchService
    {
        private readonly IEnumerable<Match> fakeMatches = new List<Match>()
        {
            new Match()
            {
                Local = "Barcelona",
                Visitor = "Manchester"
            }
        };

        public IEnumerable<Match> GetNextMatches(int max)
        {
            return this.fakeMatches;
        }
    }
}
