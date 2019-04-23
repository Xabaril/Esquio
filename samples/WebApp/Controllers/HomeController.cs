using Esquio.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Flag(FeatureName = Flags.NavigationSection)]
        [ActionName("Privacy")]
        public IActionResult PrivacyWhenFlagsIsActive()
        {
            return View();
        }

        [ActionName("Privacy")]
        public IActionResult PrivacyWhenFlagIsNotActive()
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
