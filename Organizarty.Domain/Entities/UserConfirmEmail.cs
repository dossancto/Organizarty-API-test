namespace Organizarty.Domain.Entities;

public class UserConfirmEmail
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public DateTime ValidFor { get; set; } = DateTime.Now;
}
