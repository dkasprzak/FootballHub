﻿using FootballHub.Api.Application.Response;
using FootballHub.Api.Auth;
using FootballHub.Application.Logic.User;
using FootballHub.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FootballHub.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class UserController : BaseController
{
    private readonly CookieSettings _cookieSettings;
    private readonly JwtManager _jwtManager;
    private readonly IAntiforgery _antiforgery;

    public UserController(ILogger<UserController> logger, IOptions<CookieSettings> cookieSettings, 
        JwtManager jwtManager,
        IAntiforgery antiforgery,
        IMediator mediator) : base(logger, mediator)
    {
        _cookieSettings = cookieSettings != null ? cookieSettings.Value : null;
        _jwtManager = jwtManager;
        _antiforgery = antiforgery;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateUserWithAccountCommand.Request model)
    {
        var createAccountResult = await _mediator.Send(model);
        var token = _jwtManager.GenerateUserToken(createAccountResult.UserId);
        SetTokenCookie(token);
        return Ok(new JwtToken{AccessToken = token});
    }
    
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginCommand.Request model)
    {
        var loginResult = await _mediator.Send(model);
        var token = _jwtManager.GenerateUserToken(loginResult.UserId);
        SetTokenCookie(token);
        return Ok(new JwtToken {AccessToken = token});
    }
    
    [HttpPost]
    public async Task<ActionResult> Logout([FromBody] LogoutCommand.Request model)
    {
        var logoutResult = await _mediator.Send(model);
        DeleteTokenCookie();        
        return Ok(logoutResult);
    }

    [HttpPatch]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand.Request model)
    {
        var changePasswordResult = await _mediator.Send(model);
        return Ok(changePasswordResult);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetLoggedInUser()
    {
        var data = await _mediator.Send(new LoggedInUserQuery.Request());
        return Ok(data);
    }
    
    [HttpGet]
    public async Task<ActionResult> AntiforgeryToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(tokens.RequestToken);
    }

    private void SetTokenCookie(string token)
    {
        var cookieOption = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTimeOffset.Now.AddDays(30),
            SameSite = SameSiteMode.Lax
        };

        if (_cookieSettings != null)
        {
            cookieOption = new CookieOptions()
            {
                HttpOnly = cookieOption.HttpOnly,
                Expires = cookieOption.Expires,
                Secure = cookieOption.Secure,
                SameSite = cookieOption.SameSite
            };
        }
        Response.Cookies.Append(CookieSettings.CookieName, token, cookieOption);
    }

    private void DeleteTokenCookie()
    {
        Response.Cookies.Delete(CookieSettings.CookieName, new CookieOptions{HttpOnly = true});
    }
}
