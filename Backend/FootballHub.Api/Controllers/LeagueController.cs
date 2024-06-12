using FootballHub.Application.Logic.League;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class LeagueController : BaseController
{
    public LeagueController(ILogger<LeagueController> logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpPost]
    public async Task<ActionResult> CreateLeague([FromForm] CreateLeagueCommand.Request model)
    {
        var leagueResult = await _mediator.Send(model);
        return Ok(leagueResult);
    }
}
