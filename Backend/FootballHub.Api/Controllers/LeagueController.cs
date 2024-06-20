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
    public async Task<ActionResult> CreateOrUpdateLeague([FromForm] CreateOrUpdateLeagueCommand.Request model)
    {
        var result = await _mediator.Send(model);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteLeague([FromBody] DeleteLeagueCommand.Request model)
    {
        var result = await _mediator.Send(model);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetLeagues()
    {
        var result = await _mediator.Send(new GetLeagueListQuery.Request());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetLeagueDetails([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetLeagueDetailQuery.Request { Id = id });
        return Ok(result);
    }
}
