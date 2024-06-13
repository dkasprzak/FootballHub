using FluentValidation;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.Country;

public static class CountryListQuery
{
    public class Request : IRequest<List<Result>>;
    
    public class Result
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
    }
    
    public class Handler : BaseQueryHandler, IRequestHandler<Request, List<Result>>
    {
        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<List<Result>> Handle(Request request, CancellationToken cancellationToken)
        {
            var countries = await _applicationDbContext.Countries.ToListAsync();
            var result = countries.Select(c => new Result
            {
                Id = c.Id,
                Name = c.Name,
                ShortName = c.ShortName
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
