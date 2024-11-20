namespace Shared.Infrastructure.Ciphers;

public class CipherOptions
{
    public const string Section = nameof(CipherOptions);
    public string Key { get; init; } = default!;
    public string Salt { get; init; } = default!;
}