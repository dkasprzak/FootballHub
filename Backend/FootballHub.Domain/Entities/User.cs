using FootballHub.Domain.Common;

namespace FootballHub.Domain.Entities;

public class User : DomainEntity
{
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public DateTimeOffset RegisterDate { get; set; }
    public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();
}
