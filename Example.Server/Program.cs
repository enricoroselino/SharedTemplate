using Scalar.AspNetCore;
using Shared;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddSharedConfiguration()
    .AddCarterFromAssemblies(typeof(Program).Assembly)
    .AddMediatorFromAssemblies(typeof(Program).Assembly)
    .AddQuartzFromAssemblies(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSharedConfiguration();
app.Run();