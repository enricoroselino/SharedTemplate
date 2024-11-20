using Shared;
using Shared.Documentation;
using Shared.Extensions;

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