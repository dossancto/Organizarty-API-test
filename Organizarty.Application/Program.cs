using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Organizarty.Infra.Data.Contexts;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

string GetConnectionString()
{
    string? envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

    if (envConnectionString is not null)
    {
        Console.WriteLine("Using Environment database");
        return envConnectionString;
    }

    // TODO: Try change "Console.WriteLine()" to Logger.
    Console.WriteLine("Using development database");

    if (builder is null) throw new SystemException("Builder was not created");

    return builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connect string not found");
}


// Connect to Database.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(GetConnectionString(),
        new MySqlServerVersion(new Version(8, 0, 26))));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
