using FootballHub.Domain.Common;

namespace FootballHub.Domain.Entities;

public class League : DomainEntity
{
    public string LeagueName { get; set; } = "";
    public string Logo { get; set; } = "";
    public string LogoFileName { get; set; } = "";
    public int CountryId { get; set; } 
    public Country Country { get; set; } = default!;
    public DateTimeOffset CreateDate { get; set; }
}
