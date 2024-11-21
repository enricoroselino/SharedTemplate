using Shared.Contracts.CQRS;

namespace Example.Server.Whatever.Features.TextCipher;

public record TextCipherCommand(string Message) : ICommand<string>;