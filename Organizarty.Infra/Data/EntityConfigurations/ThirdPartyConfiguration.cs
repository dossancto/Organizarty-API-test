using Organizarty.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organizarty.Infra.Data.EntityConfigurations;

public class ThirdPartyConfiguration : IEntityTypeConfiguration<ThirdParty>
{
    public void Configure(EntityTypeBuilder<ThirdParty> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Salt).IsRequired();

        builder.Property(x => x.CNPJ).IsRequired().HasMaxLength(14);

        builder.Property(x => x.ContactPhone).IsRequired();
        builder.Property(x => x.ProfissionalPhone).IsRequired();

        builder.Ignore(x => x.Tag);
    }
}
