namespace Shared.Infrastructure.Ciphers;

public interface ITextCipher
{
    public Task<string> Encrypt(string plainText, CancellationToken cancellationToken = default);
    public Task<string> Decrypt(string cipherText, CancellationToken cancellationToken = default);
}