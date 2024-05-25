using FootballHub.Domain.Common;

namespace FootballHub.Domain.Entities;

public class Account : DomainEntity
{
    public required string Name { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
}
