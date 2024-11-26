using NeoSmart.Utils;
using Shared.Contracts.CQRS;
using Shared.Extensions;
using Shared.Infrastructure.Ciphers;

namespace Example.Server.Whatever.Features.FileCipher;

public class FileCipherCommandHandler : ICommandHandler<FileCipherCommand, FileCipherCommandResult>
{
    private readonly IFileCipher _fileCipher;

    public FileCipherCommandHandler(IFileCipher fileCipher)
    {
        _fileCipher = fileCipher;
    }

    public async Task<FileCipherCommandResult> Handle(FileCipherCommand command, CancellationToken cancellationToken)
    {
        await using var inputStream = command.File.OpenReadStream();
        var plainDataBuffer = inputStream.GetBuffer();
        var encrypted = await _fileCipher.Encrypt(inputStream, cancellationToken);

        await using var simInputStream = new MemoryStream(encrypted, writable: false);
        var decrypted = await _fileCipher.Decrypt(simInputStream, cancellationToken);

        return new FileCipherCommandResult(plainDataBuffer.SequenceEqual(decrypted));
    }
}