using FootballHub.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace FootballHub.Domain.Entities;

public class League : DomainEntity
{
    public required string LeagueName { get; set; }
    public required string Logo { get; set; }
    public required string LogoFileName { get; set; }
    public required int CountryId { get; set; }
    public Country Country { get; set; } = default!;
    public DateTimeOffset CreateDate { get; set; }
}
