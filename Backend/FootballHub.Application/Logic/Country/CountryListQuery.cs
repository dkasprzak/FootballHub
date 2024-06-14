using FluentValidation;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.Country;

public static class CountryListQuery
{
    public class Request : IRequest<Result>;
    
    public class Result
    {
        public List<Country> Countries { get; set; } = new();
        public record Country
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public required string ShortName { get; set; }
        }
    }
    
    public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
    {
        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var countries = await _applicationDbContext.Countries
                .Select(x => new Result.Country
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShortName = x.ShortName
                })
                .ToListAsync();

            return new Result
            {
                Countries = countries
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
