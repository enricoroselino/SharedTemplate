using Carter;
using MediatR;

namespace Example.Server.Whatever.Features.FileCipher;

public class FileCipherEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/filecipher", async (IFormFile file, ISender mediator, CancellationToken cancellationToken) =>
            {
                var command = new FileCipherCommand(file);
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .DisableAntiforgery();
    }
}