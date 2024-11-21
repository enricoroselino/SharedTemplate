using Microsoft.Extensions.Options;
using Shared.Infrastructure.Ciphers.Algorithms.Aes;

namespace Shared.Infrastructure.Ciphers;

public interface IFileCipher
{
    public Task<MemoryStream> Encrypt(Stream input, CancellationToken cancellationToken = default);
    public Task<MemoryStream> Decrypt(Stream input, CancellationToken cancellationToken = default);
}

public class FileCipher : IFileCipher
{
    private readonly IOptions<AesCipherOptions> _options;
    private IAesCipher CipherDefined => new AesCbcImplementation(_options);

    public FileCipher(IOptions<AesCipherOptions> options)
    {
        _options = options;
    }

    public async Task<MemoryStream> Encrypt(Stream input, CancellationToken cancellationToken = default)
    {
        await using var cipher = CipherDefined;
        return await cipher.Encrypt(input, cancellationToken);
    }

    public async Task<MemoryStream> Decrypt(Stream input, CancellationToken cancellationToken = default)
    {
        await using var cipher = CipherDefined;
        return await cipher.Decrypt(input, cancellationToken);
    }
}