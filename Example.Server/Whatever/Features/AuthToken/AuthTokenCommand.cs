using Shared.Contracts.CQRS;
using Shared.Contracts.Models;

namespace Example.Server.Whatever.Features.AuthToken;

public record AuthTokenCommand(string UserName, string Password) : ICommand<AuthCommandResult>;

public record AuthCommandResult(TokenPairResult Tokens);