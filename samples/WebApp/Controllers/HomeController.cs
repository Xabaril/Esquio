using Esquio.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatchService matchService;

        public HomeController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        public IActionResult Index()
        {
            this.matchService.GetNextMatches(10);
            return View();
        }

        [FlagSwitch(FeatureName = Flags.Privacy)]
        [ActionName("Privacy")]
        public IActionResult PrivacyWhenFlagsIsActive()
        {
            return View();
        }

        [ActionName("Privacy")]
        public IActionResult PrivacyWhenFlagIsNotActive()
        {
            return RedirectToAction(nameof(Index));
        }
        
        [Flag(FeatureName = Flags.FeaturedContent)]
        public IActionResult Featured()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
