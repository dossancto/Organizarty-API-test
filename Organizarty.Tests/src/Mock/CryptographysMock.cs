using Organizarty.Adapters;

namespace Organizarty.Tests.src.Mock;

public class CryptographysMock : ICryptographys
{
    public byte[] GenenateSalt()
    {
        throw new NotImplementedException();
    }

    public (string HashedPassword, string Password) Hash(string password, byte[] salt)
    {
        throw new NotImplementedException();
    }

    public (string HashedPassword, string Password) HashPassword(string password)
    => ("Some salt", password);

    public bool VerifyPassword(string password, string storedHashedPassword, string storedSalt)
      => password == storedHashedPassword;
}
