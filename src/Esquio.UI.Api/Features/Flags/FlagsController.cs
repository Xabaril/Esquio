using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Esquio.UI.Api.Features.Flags
{
    [Authorize]
    [ApiController]
    public class FlagsController : ControllerBase
    {
        [HttpGet]
        [Route("api/v1/product/{id:int}/flags/{id:int}")]
        public IActionResult GetBy(int productId, int flagId)
        {
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/product/{id:int}/flags")]
        public IActionResult GetBy(int productId)
        {
            return Ok();
        }

        [HttpPost]
        [Route("api/v1/product/{id:int}/flags")]
        public IActionResult Add(int productId)
        {
            return Created(string.Empty, null);
        }

        [HttpPut]
        [Route("api/v1/features")]
        public IActionResult Update()
        {
            return Ok();
        }

        [HttpPut]
        [Route("api/v1/product/{id:int}/flags/{id:int}/rollout")]
        public IActionResult Rollout(int productId, int flagId)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/features/{id:int}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
