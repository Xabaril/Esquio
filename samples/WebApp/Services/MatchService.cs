using System.Collections.Generic;
using System.Linq;
using WebApp.Models;
using WebApp.Services.Fakes;

namespace WebApp.Services
{
    public class MatchService : IMatchService
    {
        private readonly IEnumerable<Match> fakeMatches = FakeMatch.Matches;
        private readonly IEnumerable<MatchMinute> fakeMatchMinutes = FakeMatch.Minutes;
        public IEnumerable<Match> GetNextMatches(int max)
        {
            return this.fakeMatches.Take(max);
        }

        public Match Get(int id)
        {
            var match = this.fakeMatches.FirstOrDefault(x => x.Id == id);
            match.Minutes = this.fakeMatchMinutes.Reverse();

            return match;
        }
    }
}
