using Esquio.UI.Api.Infrastructure.Authorization;
using Esquio.UI.Api.Shared.Models.Statistics.Configuration;
using Esquio.UI.Api.Shared.Models.Statistics.Success;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.UI.Api.Scenarios.Statistics
{
    [Authorize]
    [ApiVersion("3.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet()]
        [Route("configuration")]
        [Authorize(Policies.Reader)]
        public async Task<IActionResult> GetConfigurationStatistics(CancellationToken cancellationToken = default)
        {
            var statistics = await _mediator.Send(new ConfigurationStatisticsRequest(), cancellationToken);

            return Ok(statistics);
        }

        [HttpGet()]
        [Route("success")]
        [Authorize(Policies.Reader)]
        public async Task<IActionResult> GetSuccessStatistics(CancellationToken cancellationToken = default)
        {
            var statistics = await _mediator.Send(new SuccessStatisticsRequest(), cancellationToken);

            return Ok(statistics);
        }
    }
}
