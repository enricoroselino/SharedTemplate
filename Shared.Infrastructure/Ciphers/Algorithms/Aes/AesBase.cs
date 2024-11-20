using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Helpers;

namespace Shared.Infrastructure.Ciphers.Algorithms.Aes;

public abstract class AesBase : IDisposable, IAsyncDisposable
{
    protected readonly SymmetricAlgorithm BaseCipher;
    private const int KeyDefinedLength = 32;
    private bool _disposed;

    protected byte[] Key
    {
        get => BaseCipher.Key;
        private init => BaseCipher.Key = value;
    }

    protected AesBase(IOptions<CipherOptions> options)
    {
        var saltedKey = CryptoHelper.DerivationKey(options.Value.Key, options.Value.Salt, KeyDefinedLength);
        ArgumentOutOfRangeException.ThrowIfNotEqual(saltedKey.Length, KeyDefinedLength);

        BaseCipher = System.Security.Cryptography.Aes.Create();
        BaseCipher.Padding = PaddingMode.PKCS7;
        BaseCipher.KeySize = 256;
        Key = saltedKey;
    }

    // Synchronous Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Asynchronous Dispose
    public async ValueTask DisposeAsync()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        await ValueTask.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            // Dispose managed resources here
            BaseCipher.Dispose();
        }

        _disposed = true;
    }

    // Finalizer
    ~AesBase()
    {
        Dispose(false);
    }
}