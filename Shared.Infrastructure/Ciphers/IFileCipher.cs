namespace Shared.Infrastructure.Ciphers;

public interface IFileCipher
{
    public Task<byte[]> Encrypt(Stream input, CancellationToken cancellationToken = default);
    public Task<byte[]> Decrypt(Stream input, CancellationToken cancellationToken = default);
}