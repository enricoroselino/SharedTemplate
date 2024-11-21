using System.Text;
using Microsoft.Extensions.Options;
using NeoSmart.Utils;
using Shared.Infrastructure.Ciphers.Algorithms.Aes;

namespace Shared.Infrastructure.Ciphers;

public interface ITextCipher
{
    public Task<string> Encrypt(string plainText, CancellationToken cancellationToken = default);
    public Task<string> Decrypt(string cipherText, CancellationToken cancellationToken = default);
}

public class TextCipher : ITextCipher
{
    private readonly IOptions<AesCipherOptions> _options;
    private IAesCipher CipherDefined => new AesCbcImplementation(_options);

    public TextCipher(IOptions<AesCipherOptions> options)
    {
        _options = options;
    }

    public async Task<string> Encrypt(string plainText, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);
        using var inputStream = new MemoryStream(bytes);

        await using var cipher = CipherDefined;
        using var result = await cipher.Encrypt(inputStream, cancellationToken);
        return UrlBase64.Encode(result.ToArray());
    }

    public async Task<string> Decrypt(string cipherText, CancellationToken cancellationToken = default)
    {
        var bytes = UrlBase64.Decode(cipherText);
        using var inputStream = new MemoryStream(bytes);

        await using var cipher = CipherDefined;
        using var result = await cipher.Decrypt(inputStream, cancellationToken);
        return Encoding.UTF8.GetString(result.ToArray());
    }
}