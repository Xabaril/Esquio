using Esquio.UI.Api.Features.Products.Add;
using Esquio.UI.Api.Features.Products.Delete;
using Esquio.UI.Api.Features.Products.Details;
using Esquio.UI.Api.Features.Products.List;
using Esquio.UI.Api.Features.Products.Update;
using Esquio.UI.Api.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        [Route("api/v1/products")]
        [Authorize(Policies.Read)]
        public async Task<IActionResult> List([FromQuery]ListProductRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpGet]
        //[Authorize(Policies.Read)]
        [Route("api/v1/products/{productId:int:min(1)}")]
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
        [Route("api/v1/products")]
        public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken = default)
        {
            var idProduct = await _mediator.Send(request, cancellationToken);

            return Created($"api/v1/product/{idProduct}", null);
        }

        [HttpPut]
        [Route("api/v1/products")]
        public async Task<IActionResult> Update(UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        [Route("api/v1/products/{productId:int:min(1)}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteProductRequest request)
        {
            await _mediator.Send(request);

            return NoContent();
        }
    }
}
