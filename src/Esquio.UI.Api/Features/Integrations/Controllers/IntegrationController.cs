using Esquio.UI.Api.Features.Integrations.Models;
using Microsoft.AspNetCore.Mvc;

namespace Esquio.UI.Api.Features.Integrations.Controllers
{
    [Route("api/[controller]")]
    public class IntegrationController
        : Controller
    {
        [HttpPost("product/{productName}/features/{featureName}")]
        public IActionResult AddFeature(string productName, string featureName, FeatureModel feature)
        {
            return Ok();
        }

        [HttpPost("product/{productName}/features/{featureName}/rollout")]
        public IActionResult RolloutFeature(string featureName, string productName)
        {
            return Ok();
        }

        [HttpPost("product/{productName}/features/{featureName}/enable")]
        public IActionResult EnableFeature(string featureName, string productName)
        {
            return Ok();
        }

        [HttpPost("product/{productName}/features/{featureName}/disable")]
        public IActionResult DisableFeature(string featureName, string productName)
        {
            return Ok();
        }

        [HttpPost("product/{productName}/features/{featureName}/parameters/{parameterName}")]
        public IActionResult UpsertParameter(string productName, string featureName, string parameterName, [FromBody]string parameterValue)
        {
            return Ok();
        }
    }
}
