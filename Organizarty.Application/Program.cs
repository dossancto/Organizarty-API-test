using Organizarty.Infra.Extensions;
using Organizarty.Application.Extensions;
using FluentValidation;

using Organizarty.Domain.Validators;

using DotNetEnv;
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();

builder.Services
  .AddProvidersConfiguration()
  .AddProviders();

builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
