using FootballHub.Application.Logic.Country;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballHub.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class CountryController : BaseController
{
    public CountryController(ILogger<CountryController> logger, IMediator mediator) : base(logger, mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult> GetCountries()
    {
        var result = await _mediator.Send(new CountryListQuery.Request());
        return Ok(result);
    }
}
