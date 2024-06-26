﻿using EFCoreSecondLevelCacheInterceptor;
using FootballHub.Application.Exceptions;
using FootballHub.Application.Interfaces;
using FootballHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Services;

public class CurrentAccountProvider : ICurrentAccountProvider
{
    private readonly IAuthenticationDataProvider _authenticationDataProvider;
    private readonly IApplicationDbContext _dbContext;

    public CurrentAccountProvider(IAuthenticationDataProvider authenticationDataProvider, IApplicationDbContext dbContext)
    {
        _authenticationDataProvider = authenticationDataProvider;
        _dbContext = dbContext;
    }
    
    public async Task<int?> GetAccountId()
    {
        var userId = _authenticationDataProvider.GetUserId();
        if (userId != null)
        {
            return await _dbContext.AccountUsers
                .Where(au => au.UserId == userId.Value)
                .OrderBy(au => au.Id)
                .Select(au => (int?)au.AccountId)
                .Cacheable()
                .FirstOrDefaultAsync();
        }
        return null;
    }
    
    public async Task<Account> GetAuthenticatedAccount()
    {
        var accountId = await GetAccountId();
        if (accountId == null)
        {
            throw new UnauthorizedException();
        }
        var account = await _dbContext.Accounts.Cacheable().FirstOrDefaultAsync(a => a.Id == accountId.Value);
        if (account == null)
        {
            throw new ErrorException("AccountDoesNotExists");
        }
        return account;
    }

}
