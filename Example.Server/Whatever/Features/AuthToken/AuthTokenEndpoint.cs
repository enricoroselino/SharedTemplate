using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            });
    }
}