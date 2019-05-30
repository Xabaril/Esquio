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
        public async Task<IActionResult> Get(int productId, [FromQuery]ListProductRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        //[HttpGet]
        //[Route("{id:int}")]
        //public IActionResult GetBy(int id)
        //{
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("")]
        //public IActionResult GetBy()
        //{
        //    return Ok();
        //}

        //[HttpPost]
        //[Route("")]
        //public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    await _mediator.Send(request, cancellationToken);
        //    return Created(string.Empty, null);
        //}

        //[HttpPut]
        //[Route("")]
        //public IActionResult Update()
        //{
        //    return Ok();
        //}

        //[HttpDelete]
        //[Route("{id:int}")]
        //public async Task<IActionResult> Delete(DeleteProductRequest request)
        //{
        //    await _mediator.Send(request);
        //    return NoContent();
        //}
    }
}
