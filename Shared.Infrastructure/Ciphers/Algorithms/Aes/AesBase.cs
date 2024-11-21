using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Helpers;

namespace Shared.Infrastructure.Ciphers.Algorithms.Aes;

public abstract class AesBase : IDisposable, IAsyncDisposable
{
    private const int KeySize = AesSizeConstant.KeySize;
    protected byte[] Key { get; }
    protected bool IsDisposed;

    protected AesBase(IOptions<AesCipherOptions> options)
    {
        var saltedKey = CryptoHelper.DerivationKey(options.Value.Key, options.Value.Salt, KeySize);
        ArgumentOutOfRangeException.ThrowIfNotEqual(saltedKey.Length, KeySize);
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
        if (IsDisposed) return;

        if (disposing)
        {
            // Dispose managed resources here
        }

        IsDisposed = true;
    }

    // Finalizer
    ~AesBase()
    {
        Dispose(false);
    }
}