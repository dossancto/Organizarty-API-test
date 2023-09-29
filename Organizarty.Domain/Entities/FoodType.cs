namespace Organizarty.Domain.Entities;

public class FoodType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public ThirdParty ThirdParty { get; set; } = default!;
    public Guid ThirdPartyId { get; set; }

    public ICollection<FoodInfo> Foods { get; set; } = default!;
}
