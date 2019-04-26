using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.Services
{
    public class MatchService : IMatchService
    {
        private readonly IEnumerable<Match> fakeMatches = new List<Match>
        {
            new Match
            {
                Id = 1,
                Local = "Real Madrid",
                Visitor = "PSG",
                Time = "20:00",
                State = MatchState.Finished,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 2,
                Local = "Barcelona",
                Visitor = "Manchester City",
                Time = "19:00",
                State = MatchState.Started,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 3,
                Local = "Dortmund",
                Visitor = "Bayern",
                Time = "21:00",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 4,
                Local = "Manchester City",
                Visitor = "Barcelona",
                Time = "19:45",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 5,
                Local = "Bayern",
                Visitor = "Dortmund",
                Time = "20:45",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 6,
                Local = "Sevilla",
                Visitor = "Liverpool",
                Time = "21:00",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 7,
                Local = "PSG",
                Visitor = "Real Madrid",
                Time = "20:00",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 8,
                Local = "Ajax",
                Visitor = "Juventus",
                Time = "19:00",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 9,
                Local = "Milan",
                Visitor = "Lyon",
                Time = "21:00",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            },
            new Match
            {
                Id = 10,
                Local = "Juventus",
                Visitor = "Ajax",
                Time = "20:30",
                State = MatchState.Pending,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam pellentesque sed dolor vitae iaculis."
            }
        };

        public IEnumerable<Match> GetNextMatches(int max)
        {
            return this.fakeMatches.Take(max);
        }

        public Match Get(int id)
        {
            return this.fakeMatches.FirstOrDefault(x => x.Id == id);
        }
    }
}
