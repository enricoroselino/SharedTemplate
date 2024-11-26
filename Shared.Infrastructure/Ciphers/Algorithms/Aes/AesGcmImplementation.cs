using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Shared.Utilities;

namespace Shared.Infrastructure.Ciphers.Algorithms.Aes;

public class AesGcmImplementation : AesBase, IAesCipher
{
    private readonly AesGcm _baseCipher;

    private const int NonceSize = AesSizeConstant.NonceSize;
    private const int TagSize = AesSizeConstant.TagSize;

    public AesGcmImplementation(IOptions<AesCipherSettings> options) : base(options)
    {
        _baseCipher = new AesGcm(Key, TagSize);
    }

    public async Task<MemoryStream> Encrypt(Stream request, CancellationToken cancellationToken = default)
    {
        request.Position = 0;

        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        ArgumentOutOfRangeException.ThrowIfNotEqual(nonce.Length, NonceSize);

        var tag = new byte[TagSize];
        var cipherData = new byte[request.Length];

        _baseCipher.Encrypt(nonce, request.GetBuffer(), cipherData, tag);

        var encryptedStream = new MemoryStream();
        await encryptedStream.WriteAsync(nonce, cancellationToken);
        await encryptedStream.WriteAsync(tag, cancellationToken);
        await encryptedStream.WriteAsync(cipherData, cancellationToken);

        encryptedStream.Position = 0;
        return encryptedStream;
    }

    public async Task<MemoryStream> Decrypt(Stream request, CancellationToken cancellationToken = default)
    {
        request.Position = 0;
        var messageLength = request.Length - NonceSize - TagSize;

        var nonceBuffer = new byte[NonceSize];
        var tagBuffer = new byte[TagSize];
        var cipherDataBuffer = new byte[messageLength];

        await request.ReadExactlyAsync(nonceBuffer, cancellationToken);
        await request.ReadExactlyAsync(tagBuffer, cancellationToken);
        await request.ReadExactlyAsync(cipherDataBuffer, cancellationToken);

        var decryptedData = new byte[messageLength];
        _baseCipher.Decrypt(nonceBuffer, cipherDataBuffer, tagBuffer, decryptedData);

        var decryptedStream = new MemoryStream(decryptedData, writable: false);
        return decryptedStream;
    }

    protected override void Dispose(bool disposing)
    {
        if (IsDisposed) return;

        if (disposing)
        {
            // Dispose managed resources here
            _baseCipher.Dispose();
        }

        base.Dispose(disposing);
    }
}