using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Example.Server.Whatever.Features.TextCipher;

public class TextCipherEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/textcipher",
            async ([FromQuery] string message, ISender mediator, CancellationToken cancellationToken) =>
            {
                var command = new TextCipherCommand(message);
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("TextCipher")
            .WithSummary("Check Text Cipher");;
    }
}