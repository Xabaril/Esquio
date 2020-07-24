using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Shared.Models;
using Esquio.UI.Api.Shared.Models.Products.Add;
using Esquio.UI.Api.Shared.Models.Products.AddDeployment;
using Esquio.UI.Api.Shared.Models.Products.Delete;
using Esquio.UI.Api.Shared.Models.Products.DeleteDeployment;
using Esquio.UI.Api.Shared.Models.Products.Details;
using Esquio.UI.Api.Shared.Models.Products.Export;
using Esquio.UI.Api.Shared.Models.Products.Import;
using Esquio.UI.Api.Shared.Models.Products.List;
using Esquio.UI.Api.Shared.Models.Products.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Text;
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
        [ProducesResponseType(typeof(PaginatedResult<ListProductResponseDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResult<ListProductResponseDetail>>> List([FromQuery] ListProductRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _mediator.Send(request, cancellationToken);

            return Ok(list);
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("{productName:slug}")]
        [ProducesResponseType(typeof(DetailsProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DetailsProductResponse>> Get(string productName, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(new DetailsProductRequest { ProductName = productName }, cancellationToken);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpGet]
        [Authorize(Policies.Reader)]
        [Route("{productName:slug}/export")]
        [ProducesResponseType(typeof(ExportProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExportProductResponse>> Export(string productName, CancellationToken cancellationToken = default)
        {
            var export = await _mediator.Send(new ExportProductRequest { ProductName = productName }, cancellationToken);

            return Ok(export);
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Import(ImportProductRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken = default)
        {
            var product = await _mediator.Send(request, cancellationToken);

            return Created($"api/products/{product}?api-version=2.0", null);
        }

        [HttpPut]
        [Authorize(Policies.Contributor)]
        [Route("{productName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [Route("{productName:slug:minlength(5):maxlength(200)}/deployment/{deploymentName:slug:minlength(5):maxlength(200)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDeployment(string productName, string deploymentName, CancellationToken cancellationToken = default)
        {
            var request = new DeleteDeploymentRequest()
            {
                ProductName = productName,
                DeploymentName = deploymentName
            };

            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Policies.Contributor)]
        [Route("{productName:slug:minlength(5):maxlength(200)}/deployment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDeployment(string productName, AddDeploymentRequest request, CancellationToken cancellationToken = default)
        {
            request.ProductName = productName;

            await _mediator.Send(request, cancellationToken);

            return Created($"api/products/{productName}?api-version=2.0", null);
        }
    }
}
