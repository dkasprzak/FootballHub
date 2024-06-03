﻿using FluentValidation;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Application.Logic.Abstractions;
using FootballHub.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Logic.User;

public static class CreateUserWithAccountCommand
{
    public class Request : IRequest<Result>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    
    public class Result
    {
        public required int UserId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, Result>
    {
        private readonly IPasswordManager _passwordManager;

        public Handler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext, IPasswordManager passwordManager) : base(currentAccountProvider, applicationDbContext)
        {
            _passwordManager = passwordManager;
        }

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var userExists = await _applicationDbContext.Users.AnyAsync(u => u.Email == request.Email);
            if (userExists)
            {
                throw new ErrorException("AccountWithThisEmailAlreadyExists");
            }

            var utcNow = DateTime.UtcNow;
            var user = new Domain.Entities.User
            {
                RegisterDate = utcNow,
                Email = request.Email,
                HashedPassword = ""
            };

            user.HashedPassword = _passwordManager.HashPassword(request.Password);

            _applicationDbContext.Users.Add(user);

            var account = new Domain.Entities.Account
            {
                Name = request.Email,
                CreateDate = utcNow
            };

            _applicationDbContext.Accounts.Add(account);

            var accountUser = new AccountUser()
            {
                Account = account,
                User = user
            };

            _applicationDbContext.AccountUsers.Add(accountUser);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Result
            {
                UserId = user.Id
            };
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Email).MaximumLength(100);

                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Password).MaximumLength(50);
            }
        }
    }
}
