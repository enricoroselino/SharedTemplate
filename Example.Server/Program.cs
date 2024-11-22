using Shared;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSharedConfiguration();

builder.Services
    .AddCarterFromAssemblies(typeof(Program).Assembly)
    .AddMediatorFromAssemblies(typeof(Program).Assembly)
    .AddQuartzFromAssemblies(typeof(Program).Assembly);

var app = builder.Build();

app.UseSharedConfiguration();
app.Run();