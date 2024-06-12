using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        private readonly IFileConverter _fileConverter;
        private readonly IPasswordManager _passwordManager;

        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IFileConverter fileConverter) : base(currentAccountProvider, applicationDbContext)
        {
            _fileConverter = fileConverter;
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
            var file = _fileConverter.ReadFile(request.Logo);
            var league = new Domain.Entities.League
            {
                LeagueName = request.LeagueName,
                FileName = file.FileName,
                FileType = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                Data = _fileConverter.ConvertToByteArray(file),
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
