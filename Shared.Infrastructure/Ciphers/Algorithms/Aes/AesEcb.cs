using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Shared.Infrastructure.Ciphers.Algorithms.Aes;

public class AesEcb : AesBase, ICipher
{
    public AesEcb(IOptions<CipherOptions> options) : base(options)
    {
        BaseCipher.Mode = CipherMode.ECB;
    }

    public async Task<MemoryStream> Encrypt(Stream input, CancellationToken cancellationToken = default)
    {
        input.Position = 0;

        // ECB doesnt need to generate IV
        var encryptedStream = new MemoryStream();
        using var encryptor = BaseCipher.CreateEncryptor(BaseCipher.Key, BaseCipher.IV);
        await using var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);
        await input.CopyToAsync(cryptoStream, cancellationToken);
        await cryptoStream.FlushFinalBlockAsync(cancellationToken);
        return encryptedStream;
    }

    public async Task<MemoryStream> Decrypt(Stream input, CancellationToken cancellationToken = default)
    {
        input.Position = 0;

        var decryptedStream = new MemoryStream();
        using var decryptor = BaseCipher.CreateDecryptor(BaseCipher.Key, BaseCipher.IV);
        await using var cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
        await cryptoStream.CopyToAsync(decryptedStream, cancellationToken);
        return decryptedStream;
    }
}