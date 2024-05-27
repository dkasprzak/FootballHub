namespace FootballHub.Infrastructure.Auth;

public record JwtAuthenticationOptions
{
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpireInDays { get; set; } = 30;
}
