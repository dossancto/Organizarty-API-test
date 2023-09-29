using Organizarty.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organizarty.Infra.Data.EntityConfigurations;

public class UserConfirmEmailConfiguration : IEntityTypeConfiguration<UserConfirmEmail>
{
    public void Configure(EntityTypeBuilder<UserConfirmEmail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(a => a.User)
          .WithMany()
          .HasForeignKey(a => a.UserId);
    }
}
