using Organizarty.Adapters;
using Organizarty.Infra.Providers.Cryptography;
using Organizarty.Infra.Providers.Token;
using Organizarty.Infra.Extensions;
using Organizarty.Domain.UseCases.Users;
using Organizarty.Application.Services;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Move to extention
builder.Services.AddScoped<ICryptographys, Pbkdf2>();
builder.Services.AddScoped<ITokenAdapter, JWT>();

builder.Services.AddScoped<ISignUseCase, SignService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
