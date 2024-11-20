using Shared;
using Shared.Extensions;
using Shared.Presentation.Documentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerDocumentation()
    .AddScalarDocumentation();

builder.Services
    .AddSharedConfiguration()
    .AddCarterFromAssemblies(typeof(Program).Assembly)
    .AddMediatorFromAssemblies(typeof(Program).Assembly)
    .AddQuartzFromAssemblies(typeof(Program).Assembly);

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseSharedConfiguration();
app.Run();