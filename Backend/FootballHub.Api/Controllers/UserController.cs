﻿using FootballHub.Application.Logic.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class UserController : BaseController
{
    public UserController(ILogger logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpPost]
    public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateUserWithAccountCommand.Request model)
    {
        var createAccountResult = await _mediator.Send(model);
        return Ok(createAccountResult);
    }
    
}
