using System.Text.Json;

namespace Organizarty.Domain.Entities;

public class ThirdParty
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public string? LoginEmail { get; set; }
    public string? Salt { get; set; }
    public string? Password { get; set; }
    public string? ProfissionalPhone { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? CNPJ { get; set; }

    public string? TagJSON { get; set; }

    public List<string> Tag
    {
        get => JsonSerializer.Deserialize<List<string>>(TagJSON ?? "[]") ?? new List<string>();
        set => TagJSON = JsonSerializer.Serialize(value);
    }
}
