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
    private readonly IOptions<CipherOptions> _options;
    private ICipher Cipher => new AesEcb(_options);

    public FileCipher(IOptions<CipherOptions> options)
    {
        _options = options;
    }

    public async Task<MemoryStream> Encrypt(Stream input, CancellationToken cancellationToken = default)
    {
        await using var cipher = Cipher;
        return await cipher.Encrypt(input, cancellationToken);
    }

    public async Task<MemoryStream> Decrypt(Stream input, CancellationToken cancellationToken = default)
    {
        await using var cipher = Cipher;
        return await cipher.Decrypt(input, cancellationToken);
    }
}