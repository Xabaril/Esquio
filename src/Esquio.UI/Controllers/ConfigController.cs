using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Esquio.UI.Controllers
{
  [Route("api/[controller]")]
  public class ConfigController : Controller
  {
    private readonly IConfiguration _configuration;

    public ConfigController(IConfiguration config)
    {
      _configuration = config;

    }

    [HttpGet]
    public IActionResult Get()
    {
      var settings = _configuration.GetSection("Settings").GetChildren();
      var security = _configuration.GetSection("Security").GetSection("Jwt").GetChildren();
      var result = settings.Concat(security).ToDictionary(x => x.Key, x => x.Value);

      return Ok(result);
    }
  }
}
