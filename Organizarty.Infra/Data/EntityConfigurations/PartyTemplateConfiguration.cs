using Organizarty.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organizarty.Infra.Data.EntityConfigurations;

public class PartyTemplateConfiguration : IEntityTypeConfiguration<PartyTemplate>
{
    public void Configure(EntityTypeBuilder<PartyTemplate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ExpectedGuests).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(256);
    }
}
