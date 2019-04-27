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
            var match = this.matchService.Get(id);

            if (!User.Identity.IsAuthenticated || match.State != Models.MatchState.Started)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(match);
        }
    }
}
