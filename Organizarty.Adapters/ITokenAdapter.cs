namespace Organizarty.Adapters;

public interface ITokenAdapter
{
  string GenerateToken(string userId, string username);
}
