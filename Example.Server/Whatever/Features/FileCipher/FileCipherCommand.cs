using Shared.Contracts.CQRS;

namespace Example.Server.Whatever.Features.FileCipher;

public record FileCipherCommand(IFormFile File) : ICommand<FileCipherCommandResult>;

public record FileCipherCommandResult(bool IsSequenceSame);