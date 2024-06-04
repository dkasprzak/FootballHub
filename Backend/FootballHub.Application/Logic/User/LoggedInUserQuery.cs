using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.User;

public static class LoggedInUserQuery
{
    public class Request : IRequest<Result>;

    public class Result
    {
        public required string Email { get; set; }
    }

    public class Handler : BaseQueryHandler, IRequestHandler<Request, Result>
    {
        private readonly IAuthenticationDataProvider _authenticationDataProvider;

        public Handler(ICurrentAccountProvider currentAccountProvider, 
            IApplicationDbContext applicationDbContext, IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
        {
            _authenticationDataProvider = authenticationDataProvider;
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var userId = _authenticationDataProvider.GetUserId();
            if (userId.HasValue)
            {
                var user = await _applicationDbContext.Users.Cacheable().FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    return new Result()
                    {
                        Email = user.Email
                    };
                }
            }

            throw new UnauthorizedException();
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
            }
        }
    }
}
