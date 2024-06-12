using FootballHub.Domain.Common;

namespace FootballHub.Domain.Entities;

public class Country : DomainEntity
{
    public required string Name { get; set; }
    public required string ShortName { get; set; }
    public ICollection<League> Leagues = new List<League>();
}
