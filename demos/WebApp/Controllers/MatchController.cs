using Esquio.AspNetCore.Endpoints.Metadata;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MatchController : Controller
    {
        private readonly IMatchService matchService;

        public MatchController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(this.matchService.GetNextMatches(50));
        }

        [ActionName("Detail")]
        public IActionResult DetailWhenFlagsIsNotActive(int id)
        {
            var match = this.matchService.Get(id);

            if (!User.Identity.IsAuthenticated || match.State != Models.MatchState.Started)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(match);
        }

        [FeatureFilter(Name = Flags.MinutesRealTime)]
        [ActionName("Detail")]
        public IActionResult DetailWhenFlagsIsActive(int id)
        {
            var match = this.matchService.Get(id);

            if (!User.Identity.IsAuthenticated || match.State != Models.MatchState.Started)
            {
                return RedirectToAction("Error", "Home");
            }

            return View("DetailLive", match);
        }
    }
}
