using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Helpers;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FootballHub.Application.Helpers;

namespace FootballHub.Application.Logic.League;

public static class CreateLeagueCommand
{
    public class Request : IRequest<Result>
    {
        public required string LeagueName { get; set; }
        public required IFormFile Logo { get; set; }
        public required int CountryId { get; set; }
    }
    
    public class Result
    {
        public required int LeagueId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
    {
        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var account = await _currentAccountProvider.GetAuthenticatedAccount();
            if (account is null)
            {
                throw new UnauthorizedException();
            }
            
            var leagueExists = await _applicationDbContext.Leagues.AnyAsync(l => l.LeagueName == request.LeagueName && l.CountryId == request.CountryId);
            if (leagueExists)
            {
                throw new ErrorException("LeagueWithThisNameAndCountryAlreadyExists");
            }

            var utcNow = DateTime.UtcNow;
            var logo = request.Logo;
            var logoFileName = $"{NameConverter.FormatFileNameToSnakeCase(request.LeagueName)}{Path.GetExtension(logo.FileName)}";
            
            var league = new Domain.Entities.League
            {
                LeagueName = request.LeagueName,
                Logo = FileConverter.ConvertFileToBase64String(logo),
                LogoFileName = logoFileName,
                CreateDate = utcNow,
                CountryId = request.CountryId,
            };
            
            _applicationDbContext.Leagues.Add(league);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Result
            {
                LeagueId = league.Id
            };
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.LeagueName).NotEmpty();
                RuleFor(x => x.LeagueName).MaximumLength(200);

                RuleFor(x => x.Logo).NotEmpty();
                
                RuleFor(x => x.CountryId).NotEmpty();
            }
        }
    }
}
