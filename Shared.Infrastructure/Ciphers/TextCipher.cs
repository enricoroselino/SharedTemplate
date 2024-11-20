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
    private readonly IOptions<CipherOptions> _options;
    private ICipher Cipher => new AesEcb(_options);

    public TextCipher(IOptions<CipherOptions> options)
    {
        _options = options;
    }
    
    public async Task<string> Encrypt(string plainText, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(plainText);

        await using var cipher = Cipher;
        using var inputStream = new MemoryStream(bytes);
        await using var encryptedStream = await cipher.Encrypt(inputStream, cancellationToken);
        return UrlBase64.Encode(encryptedStream.ToArray());
    }

    public async Task<string> Decrypt(string cipherText, CancellationToken cancellationToken = default)
    {
        var bytes = UrlBase64.Decode(cipherText);

        await using var cipher = Cipher;
        using var inputStream = new MemoryStream(bytes);
        await using var decryptedStream = await cipher.Decrypt(inputStream, cancellationToken);
        return Encoding.UTF8.GetString(decryptedStream.ToArray());
    }
}