using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Extensions;
using Shared.Infrastructure.Ciphers;

namespace Example.Server.Whatever.Features.AuthToken;

public class AuthTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/authtoken",
                async ([FromBody] AuthTokenCommand command, ISender mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(command, cancellationToken);
                    return result;
                })
            .WithSummary("Get Auth Token");

        app.MapPost("/encryptedclaim",
                async (ITextCipher textCipher, HttpContext context) =>
                {
                    var userId = await context.User.GetUserId(textCipher);
                    return Results.Ok(userId);
                })
            .RequireAuthorization()
            .WithSummary("Read Encrypted Claim");
    }
}