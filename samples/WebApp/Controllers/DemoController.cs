using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult SomeAction()
        {
            return Ok();
        }
    }
}
