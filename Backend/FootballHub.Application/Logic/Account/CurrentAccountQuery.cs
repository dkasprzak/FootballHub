using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Application.Logic.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.Account;

public static class CurrentAccountQuery
{
    public class Request : IRequest<Result>;

    public class Result
    {
        public required string Name { get; set; }
    }

    public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
    {

        public Handler(ICurrentAccountProvider currentAccountProvider, 
            IApplicationDbContext applicationDbContext) : base(currentAccountProvider, applicationDbContext)
        {
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var account = await _currentAccountProvider.GetAuthenticatedAccount();
            return new Result()
            {
                Name = account.Name
            };
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
            }
        }
    }
}
