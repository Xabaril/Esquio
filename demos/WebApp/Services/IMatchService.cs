using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IMatchService
    {
        IEnumerable<Match> GetNextMatches(int max);
        Match Get(int id);
    }
}
