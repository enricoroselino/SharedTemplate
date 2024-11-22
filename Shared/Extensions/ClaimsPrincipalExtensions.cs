using System.Security.Claims;
using Shared.Infrastructure.Ciphers;

namespace Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static async Task<Guid> GetUserId(this ClaimsPrincipal? user, ITextCipher textCipher)
    {
        var encryptedUserId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ArgumentNullException.ThrowIfNull(encryptedUserId);

        var userId = await textCipher.Decrypt(encryptedUserId);
        return Guid.Parse(userId);
    }
}