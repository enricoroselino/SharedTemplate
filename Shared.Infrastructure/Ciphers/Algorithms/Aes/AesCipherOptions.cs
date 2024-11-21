namespace Shared.Infrastructure.Ciphers.Algorithms.Aes;

public class AesCipherOptions
{
    public const string Section = nameof(AesCipherOptions);
    public string Key { get; init; } = default!;
    public string Salt { get; init; } = default!;
}