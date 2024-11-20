namespace Shared.Infrastructure.Ciphers;

public interface ICipher : IDisposable, IAsyncDisposable
{
    public Task<MemoryStream> Encrypt(Stream input, CancellationToken cancellationToken = default);
    public Task<MemoryStream> Decrypt(Stream input, CancellationToken cancellationToken = default);
}