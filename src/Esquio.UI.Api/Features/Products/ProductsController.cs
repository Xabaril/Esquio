using Esquio.UI.Api.Features.Products.Add;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Esquio;
using System.Threading.Tasks;
using System.Threading;
using Esquio.UI.Api.Features.Products.Delete;
using System;
using Esquio.UI.Api.Features.Products.List;
using Esquio.UI.Api.Features.Products.Details;

namespace Esquio.UI.Api.Features.Products
{
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("api/v1/product")]
        public async Task<IActionResult> List(int productId, [FromQuery]ListProductRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpGet]
        [Route("api/v1/product/{productId:int:min(1)}")]
        public async Task<IActionResult> Get([FromRoute]DetailsProductRequest request, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(request, cancellationToken);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("api/v1/product")]
        public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken = default)
        {
            var idProduct = await _mediator.Send(request, cancellationToken);

            return Created($"api/v1/product/{idProduct}", null);
        }

        //[HttpDelete]
        //[Route("{id:int}")]
        //public async Task<IActionResult> Delete(DeleteProductRequest request)
        //{
        //    await _mediator.Send(request);
        //    return NoContent();
        //}
    }
}
