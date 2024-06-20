using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Helpers;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.League;

public static class CreateOrUpdateLeagueCommand
{
    public class Request : IRequest<Result>
    {
        public int? Id { get; set; }
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

            Domain.Entities.League? model = null;
            
            var leagueExists = await _applicationDbContext.Leagues
                .AnyAsync(l => l.LeagueName == request.LeagueName && l.CountryId == request.CountryId && (!request.Id.HasValue || l.Id != request.Id.Value));
            if (leagueExists)
            {
                throw new ErrorException("LeagueWithThisNameAndCountryAlreadyExists");
            }
            
            var utcNow = DateTime.UtcNow;
            var logo = request.Logo;
            var logoFileName = $"{NameConverter.FormatFileNameToSnakeCase(request.LeagueName)}{Path.GetExtension(logo.FileName)}";
            
            if (request.Id.HasValue)
            {
                model = await _applicationDbContext.Leagues.FirstOrDefaultAsync(x => x.Id == request.Id);
            }
            else
            {
                model = new Domain.Entities.League()
                {
                    CreateDate = utcNow
                };
                _applicationDbContext.Leagues.Add(model);
            }

            if (model == null)
            {
                throw new UnauthorizedException();
            }

            model.LeagueName = request.LeagueName;
            model.Logo = FileConverter.ConvertFileToBase64String(logo);
            model.LogoFileName = logoFileName;
            model.CountryId = request.CountryId;
            
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Result
            {
                LeagueId = model.Id
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
