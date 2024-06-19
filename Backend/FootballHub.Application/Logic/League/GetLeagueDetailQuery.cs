using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.League;

public static class GetLeagueDetailQuery
{
    public class Request : IRequest<Result>
    {
        public required int Id { get; set; }
    }
    
    public class Result
    {
        public required int Id { get; set; }
        public required string LeagueName { get; set; }
        public required string CountryName { get; set; }
        public required string CountryShortName { get; set; }
        public required string Logo { get; set; }
        public required string LogoFileName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
    
    public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
    {
        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var league = await _applicationDbContext.Leagues
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (league is null)
            {
                throw new NotFoundException();
            }
            
            return new Result()
            {
                Id = league.Id,
                LeagueName = league.LeagueName,
                CountryName = league.Country.Name,
                CountryShortName = league.Country.ShortName,
                Logo = league.Logo,
                LogoFileName = league.LogoFileName,
                CreateDate = league.CreateDate
            };
        }
    }
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
