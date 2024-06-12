using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.User;

public static class ChangePasswordCommand
{
    public class Request : IRequest<Result>
    { 
        public required string Password { get; set; }
    }
    
    public class Result
    {
        public required int UserId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
    {
        private readonly IPasswordManager _passwordManager;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;

        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IPasswordManager passwordManager, IAuthenticationDataProvider authenticationDataProvider) : base(currentAccountProvider, applicationDbContext)
        {
            _passwordManager = passwordManager;
            _authenticationDataProvider = authenticationDataProvider;
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var currentUserId = _authenticationDataProvider.GetUserId();
            if (currentUserId is null)
            {
                throw new UnauthorizedException();
            }
            var userData = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);
            if (userData is null)
            {
                throw new ErrorException("UserDoesNotExists");
            }
            
            userData.HashedPassword = _passwordManager.HashPassword(request.Password);

            _applicationDbContext.Users.Entry(userData).Property(u => u.HashedPassword).IsModified = true;
            
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Result
            {
                UserId = userData.Id
            };
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Password).MaximumLength(50);
            }
        }
    }
}
