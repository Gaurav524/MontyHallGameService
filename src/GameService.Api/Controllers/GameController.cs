using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GameService.Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly ILogger<GameController> _logger;

        public GameController(ISender mediator, ILogger<GameController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("wins")]
        [ProducesResponseType(typeof(GameResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<GameResponse>>> GetWins(
            [FromBody] GameRequest gameRequest)
        {
            try
            {
                var wins = await _mediator.Send(gameRequest);
                return Ok(wins);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return Ok(new GameResponse(gameRequest.NumberOfSimulations));
        }
    }
}
