using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Esquio.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebAppCustomToggle.Models;

namespace WebAppCustomToggle.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFeatureService _featureService;

        public HomeController(IFeatureService featureService)
        {
            _featureService = featureService ?? throw new ArgumentNullException(nameof(featureService));
        }
        public async Task<IActionResult> Index()
        {
            if ( await _featureService.IsEnabledAsync(featureName:"IberianRequests"))
            {
                return View();
            }

            return NotFound();
        }

        public IActionResult Privacy()
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
