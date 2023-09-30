using Organizarty.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Organizarty.Infra.Data.EntityConfigurations;

public class FoodGroupConfiguration : IEntityTypeConfiguration<FoodGroup>
{
    public void Configure(EntityTypeBuilder<FoodGroup> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Note).HasMaxLength(256);
    }
}
