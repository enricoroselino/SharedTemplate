using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentValidation;
using Shared.Contracts.CQRS;
using Shared.Infrastructure.Providers;
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

    public AuthTokenCommandHandler(ITokenProvider tokenProvider, IGuidProvider guidProvider)
    {
        _tokenProvider = tokenProvider;
        _guidProvider = guidProvider;
    }

    public async Task<AuthCommandResult> Handle(AuthTokenCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, "John Doe Tampubolon"),
            new Claim(JwtRegisteredClaimNames.EmailVerified, "john.doe@example.com"),
            new Claim(JwtRegisteredClaimNames.NameId, _guidProvider.NewRandom().ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, _guidProvider.NewRandom().ToString())
        };

        var tokenPair = _tokenProvider.GenerateTokenPair(claims);
        var result = new AuthCommandResult(tokenPair);
        return await Task.FromResult(result);
    }
}