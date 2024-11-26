﻿using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Shared.Infrastructure.Ciphers.Algorithms.Aes;

public sealed class AesCbcImplementation : AesBase, IAesCipher
{
    private readonly SymmetricAlgorithm _baseCipher;
    private const int IvSize = AesSizeConstant.IvSize;

    public AesCbcImplementation(IOptions<AesCipherSettings> options) : base(options)
    {
        _baseCipher = System.Security.Cryptography.Aes.Create();
        _baseCipher.Padding = PaddingMode.PKCS7;
        _baseCipher.KeySize = 256;
        _baseCipher.Mode = CipherMode.CBC;
        _baseCipher.Key = Key;
    }

    public async Task<MemoryStream> Encrypt(Stream request, CancellationToken cancellationToken = default)
    {
        request.Position = 0;

        _baseCipher.GenerateIV();
        ArgumentOutOfRangeException.ThrowIfNotEqual(_baseCipher.IV.Length, IvSize);

        var encryptedStream = new MemoryStream();
        await encryptedStream.WriteAsync(_baseCipher.IV, cancellationToken);

        using var encryptor = _baseCipher.CreateEncryptor(_baseCipher.Key, _baseCipher.IV);
        await using var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);
        await request.CopyToAsync(cryptoStream, cancellationToken);
        await cryptoStream.FlushFinalBlockAsync(cancellationToken);
        return encryptedStream;
    }

    public async Task<MemoryStream> Decrypt(Stream request, CancellationToken cancellationToken = default)
    {
        request.Position = 0;

        var ivBuffer = new byte[AesSizeConstant.IvSize];
        await request.ReadExactlyAsync(ivBuffer, cancellationToken);
        ArgumentOutOfRangeException.ThrowIfNotEqual(ivBuffer.Length, IvSize);
        _baseCipher.IV = ivBuffer;
        
        var decryptedStream = new MemoryStream();
        using var decryptor = _baseCipher.CreateDecryptor(_baseCipher.Key, _baseCipher.IV);
        await using var cryptoStream = new CryptoStream(request, decryptor, CryptoStreamMode.Read);
        await cryptoStream.CopyToAsync(decryptedStream, cancellationToken);
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