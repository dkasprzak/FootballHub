using FootballHub.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace FootballHub.Domain.Entities;

public class League : DomainEntity
{
    public required string LeagueName { get; set; }
    public required string FileName { get; set; }
    public string FileType { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Data { get; set; }
    public required int CountryId { get; set; }
    public Country Country { get; set; } = default!;
    public DateTimeOffset CreateDate { get; set; }
}
