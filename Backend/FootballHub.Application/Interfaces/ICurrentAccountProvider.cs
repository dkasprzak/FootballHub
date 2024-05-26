using FootballHub.Domain.Entities;

namespace FootballHub.Application.Interfaces;

public interface ICurrentAccountProvider
{
    Task<Account> GetAuthenticatedAccount();
    Task<int?> GetAccountId();
}
