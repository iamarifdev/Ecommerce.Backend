using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Ecommerce.Backend.Common.Helpers
{
  public class Hash
  {
    public static string Create(string password, string salt)
    {
      var valueBytes = KeyDerivation.Pbkdf2(
        password: password,
        salt: Encoding.UTF8.GetBytes(salt),
        prf: KeyDerivationPrf.HMACSHA512,
        iterationCount: 10000,
        numBytesRequested: 256 / 8
      );
      return Convert.ToBase64String(valueBytes);
    }

    public static bool Validate(string password, string salt, string hash)
    {
      var generatedHash = Create(password, salt);
      return generatedHash == hash;
    }
  }
}