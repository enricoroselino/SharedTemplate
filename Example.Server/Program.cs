using Shared;
using Shared.Extensions;
using Shared.Infrastructure.Ciphers;
using Shared.Infrastructure.Configurations;
using Shared.Infrastructure.Configurations.Documentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerDocumentation()
    .AddScalarDocumentation();

builder.Services
    .AddSharedConfiguration()
    .AddCiphers();

builder.Services
    .AddCarterFromAssemblies(typeof(Program).Assembly)
    .AddMediatorFromAssemblies(typeof(Program).Assembly)
    .AddQuartzFromAssemblies(typeof(Program).Assembly);

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseSharedConfiguration();
app.Run();