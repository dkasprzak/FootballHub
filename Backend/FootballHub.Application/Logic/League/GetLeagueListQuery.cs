using FluentValidation;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.League;

public static class GetLeagueListQuery
{
    public class Request : IRequest<List<Result>>;
    
    public class Result
    {
        public required int Id { get; set; }
        public required string LeagueName { get; set; }
        public required string CountryName { get; set; }
        public required string CountryShortName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
    
    public class Handler : BaseQueryHandler, IRequestHandler<Request, List<Result>>
    {
        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<List<Result>> Handle(Request request, CancellationToken cancellationToken)
        {
            var leagues = await _applicationDbContext.Leagues
                .Include(x => x.Country)
                .ToListAsync();

            var result = leagues.Select(l => new Result
            {
                Id = l.Id,
                LeagueName = l.LeagueName,
                CountryName = l.Country.Name,
                CountryShortName = l.Country.ShortName,
                CreateDate = l.CreateDate,
            }).ToList();

            return result;
        }
    }
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
        }
    }
}
