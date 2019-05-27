using Esquio.UI.Api.Features.Products.Add;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Esquio;
using System.Threading.Tasks;
using System.Threading;
using Esquio.UI.Api.Features.Products.Delete;

namespace Esquio.UI.Api.Features.Products
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator)
        {
            Ensure.Argument.NotNull(mediator, nameof(mediator));
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetBy(int id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetBy()
        {
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            await mediator.Send(request, cancellationToken);
            return Created(string.Empty, null);
        }

        [HttpPut]
        [Route("")]
        public IActionResult Update()
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(DeleteProductRequest request)
        {
            await mediator.Send(request);
            return NoContent();
        }
    }
}
