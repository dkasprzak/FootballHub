using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.League;

public static class DeleteLeagueCommand
{
    public class Request : IRequest<Result>, IRequest<Handler>
    {
        public int Id { get; set; }
    }
    
    public class Result
    {
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

            var model = await _applicationDbContext.Leagues.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (model == null)
            {
                throw new NotFoundException();
            }

            _applicationDbContext.Leagues.Remove(model);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Result();

        }
    }
    
}
