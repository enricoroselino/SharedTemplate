namespace Shared.Infrastructure.Providers.TokenProvider;

public class TokenOptions
{
    public const string Section = nameof(TokenOptions);
    public string Key { get; init; } = default!;
    public string Salt { get; init; } = default!;
    public string ValidIssuer { get; init; } = default!;
    public string ValidAudience { get; init; } = default!;
    public TimeSpan ExpirationSpan { get; init; }
}