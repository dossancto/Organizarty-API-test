using Organizarty.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organizarty.Infra.Data.EntityConfigurations;

public class FoodInfoConfiguration : IEntityTypeConfiguration<FoodInfo>
{
    public void Configure(EntityTypeBuilder<FoodInfo> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Flavour).IsRequired().HasMaxLength(35);
        builder.Property(x => x.Price).IsRequired();

        builder.Ignore(x => x.Images);
    }
}
