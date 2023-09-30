using Organizarty.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organizarty.Infra.Data.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Ignore(x => x.Roles);

        builder.Property(x => x.Email).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Salt).IsRequired();
    }
}
