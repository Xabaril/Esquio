using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService matchService;

        public MatchController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        public IActionResult Index()
        {
            return View(this.matchService.GetNextMatches(5));
        }

        public IActionResult Detail(int id)
        {
            return View(this.matchService.Get(id));
        }
    }
}
