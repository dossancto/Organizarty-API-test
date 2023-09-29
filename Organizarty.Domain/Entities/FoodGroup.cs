using System.Text.Json;

namespace Organizarty.Domain.Entities;

public class FoodGroup
{
    public Guid Id { get; set; }

    public string? Flavour { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }

    public string? ImagesJson { get; set; }

    public List<string> Images
    {
        get => JsonSerializer.Deserialize<List<string>>(ImagesJson ?? "[]") ?? new List<string>();
        set => ImagesJson = JsonSerializer.Serialize(value);
    }

    public Guid FoodTypeId { get; set; } = default!;
    public FoodType FoodType { get; set; } = default!;

    public Guid PartyTemplateId { get; set; } = default!;
    public PartyTemplate PartyTemplate { get; set; } = default!;
}
