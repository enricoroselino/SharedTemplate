namespace Shared.Token;

public class TokenSettings
{
    public const string Section = nameof(TokenSettings);
    public string Key { get; init; }
    public string ValidIssuer { get; init; }
    public string ValidAudience { get; init; }
}