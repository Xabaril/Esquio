using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Scenarios.Products.Add;
using Esquio.UI.Api.Scenarios.Products.AddRing;
using Esquio.UI.Api.Scenarios.Products.Delete;
using Esquio.UI.Api.Scenarios.Products.DeleteRing;
using Esquio.UI.Api.Scenarios.Products.Details;
using Esquio.UI.Api.Scenarios.Products.List;
using Esquio.UI.Api.Scenarios.Products.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Products
{
    [Authorize]
    [ApiVersion("3.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policies.Reader)]
        [ProducesResponseType(typeof(ListProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListProductResponse>> List([FromQuery]ListProductRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("{productName:slug}")]
        [ProducesResponseType(typeof(DetailsProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsProductResponse>> Get(string productName, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(new DetailsProductRequest { ProductName = productName }, cancellationToken);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(request, cancellationToken);

            return Created($"api/products/{product}?api-version=2.0", null);
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("{productName:slug:minlength(5):maxlength(200)}/ring")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRing(string productName, AddRingRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;

            await _mediator.Send(request, cancellationToken);

            return Created($"api/products/{productName}?api-version=2.0", null);
        }

        [HttpPut]
        [Authorize(Policies.Contributor)]
        [Route("{productName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string productName, UpdateProductRequest request, CancellationToken cancellationToken = default)
        {
            request.CurrentName = productName;

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policies.Contributor)]
        [Route("{productName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string productName, CancellationToken cancellationToken = default)
        {
            var request = new DeleteProductRequest()
            {
                ProductName = productName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Policies.Contributor)]
        [Route("{productName:slug:minlength(5):maxlength(200)}/ring/{ringName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRing(string productName, string ringName, CancellationToken cancellationToken = default)
        {
            var request = new DeleteRingRequest()
            {
                ProductName = productName,
                RingName = ringName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}
