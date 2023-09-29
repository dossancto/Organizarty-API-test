using System.Text.Json;

namespace Organizarty.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string Salt { get; set; } = default!;

    public bool EmailConfirmed { get; set; }

    // public ICollection<PartyTemplateModel> PartyTemplates { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public string? RolesJson { get; set; }

    public List<string> Roles
    {
        get => JsonSerializer.Deserialize<List<string>>(RolesJson ?? "[]") ?? new List<string>();
        // TODO: Add more validations for save new roles.
        set => RolesJson = JsonSerializer.Serialize(value);
    }
}
