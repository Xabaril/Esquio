using Esquio.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class DemoController
        :Controller
    {

        [FeatureFilter(Names = "TheFeatureName")]
        public IActionResult SomeAction()
        {
            return View();
        }
    }
}
