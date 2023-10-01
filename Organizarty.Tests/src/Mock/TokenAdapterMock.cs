using Organizarty.Adapters;

namespace Organizarty.Tests.src.Mock;

public class TokenAdapterMock : ITokenAdapter
{
    public string GenerateToken(string userId, string username) => Guid.NewGuid().ToString();
}
