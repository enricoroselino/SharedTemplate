using FluentValidation;
using Shared.Contracts.CQRS;
using Shared.Infrastructure.Ciphers;

namespace Example.Server.Whatever.Features.TextCipher;

public class TextCipherValidator : AbstractValidator<TextCipherCommand>
{
    public TextCipherValidator()
    {
        RuleFor(x => x.Message).MinimumLength(5);
    }
}

public class TextCipherCommandHandler : ICommandHandler<TextCipherCommand, string>
{
    private readonly ITextCipher _cipher;

    public TextCipherCommandHandler(ITextCipher cipher)
    {
        _cipher = cipher;
    }

    public async Task<string> Handle(TextCipherCommand request, CancellationToken cancellationToken)
    {
        var encrypted = await _cipher.Encrypt(request.Message, cancellationToken);
        var decrypted = await _cipher.Decrypt(encrypted, cancellationToken);
        return decrypted;
    }
}