using Microsoft.EntityFrameworkCore;
using Organizarty.Domain.Entities;
using Organizarty.Infra.Data.EntityConfigurations;

namespace Organizarty.Infra.Data.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserConfirmEmail> UserConfirmEmails { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
