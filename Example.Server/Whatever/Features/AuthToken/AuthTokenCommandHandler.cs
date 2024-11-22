using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Shared.Contracts.CQRS;
using Shared.Contracts.Enums;
using Shared.Infrastructure.Ciphers;
using Shared.Infrastructure.Providers;
using Shared.Infrastructure.Providers.GuidProvider;
using Shared.Infrastructure.Providers.TokenProvider;

namespace Example.Server.Whatever.Features.AuthToken;

public class AuthTokenCommandValidator : AbstractValidator<AuthTokenCommand>
{
    public AuthTokenCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class AuthTokenCommandHandler : ICommandHandler<AuthTokenCommand, AuthCommandResult>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IGuidProvider _guidProvider;
    private readonly ITextCipher _textCipher;

    public AuthTokenCommandHandler(
        ITokenProvider tokenProvider,
        IGuidProviderFactory guidProviderFactory,
        ITextCipher textCipher)
    {
        _tokenProvider = tokenProvider;
        _guidProvider = guidProviderFactory.Create(DatabaseFlavor.Mssql);
        _textCipher = textCipher;
    }

    public async Task<AuthCommandResult> Handle(AuthTokenCommand request, CancellationToken cancellationToken)
    {
        var encryptedUserId = await _textCipher.Encrypt(_guidProvider.NewSequential().ToString(), cancellationToken);

        // dont put PII data to comply GDPR like regulation
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, encryptedUserId),
            new Claim(JwtRegisteredClaimNames.Jti, _guidProvider.NewSequential().ToString())
        };

        var tokenPair = _tokenProvider.GenerateTokenPair(claims);
        var result = new AuthCommandResult(tokenPair);
        return await Task.FromResult(result);
    }
}