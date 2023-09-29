namespace Organizarty.Domain.Entities;

public class PartyTemplate
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public int ExpectedGuests { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }

    public FoodType FoodType { get; set; } = default!;
    public Guid FoodTypeId { get; set; } = default!;
}
