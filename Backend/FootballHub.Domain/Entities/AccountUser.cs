using FootballHub.Domain.Common;

namespace FootballHub.Domain.Entities;

public class AccountUser : DomainEntity
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = default!;
    public int UserId { get; set; }
    public User User { get; set; } = default!;
}
