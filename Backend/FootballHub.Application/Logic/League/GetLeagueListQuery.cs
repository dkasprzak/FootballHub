using FluentValidation;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.League;

public static class GetLeagueListQuery
{
    public class Request : IRequest<Result>;
    
    public class Result
    {
        public List<League> Leagues { get; set; } = new();
        public record League
        {
            public required int Id { get; set; }
            public required string LeagueName { get; set; }
            public required string CountryName { get; set; }
            public required string CountryShortName { get; set; }
            public DateTimeOffset CreateDate { get; set; }   
        }
    }
    
    public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
    {
        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var leagues = await _applicationDbContext.Leagues
                .Include(x => x.Country)
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new Result.League
                {
                    Id = x.Id,
                    LeagueName = x.LeagueName,
                    CountryName = x.Country.Name,
                    CountryShortName = x.Country.ShortName,
                    CreateDate = x.CreateDate
                }).ToListAsync();

            return new Result
            {
                Leagues = leagues
            };
        }
    }
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
        }
    }
}
