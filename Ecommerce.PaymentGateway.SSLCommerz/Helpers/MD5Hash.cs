using System;
using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.PaymentGateway.SSLCommerz.Helpers
{
  public class MD5Hash
  {
    /// <summary>
    /// Generate PHP like MD5 Hash for SSLCommerz IPN verfication, which is built on PHP language.
    /// 
    /// For reference: `https://stackoverflow.com/questions/37438961/c-sharp-code-php-equivalent-md5-hash`
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Generate(string text)
    {
      using(MD5 md5 = MD5CryptoServiceProvider.Create())
      {
        var asciiBytes = ASCIIEncoding.ASCII.GetBytes(text);
        var hashedBytes = md5.ComputeHash(asciiBytes);
        string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        return hashedString;
      }
    }

    public static bool Verify(string text, string hash, bool simpleComparison = false)
    {
      if (simpleComparison)
      {
        return text == hash;
      }
      using(MD5 md5 = MD5.Create())
      {
        string hashOfText = Generate(text);
        var comparer = StringComparer.OrdinalIgnoreCase;
        var isVerified = comparer.Compare(hashOfText, hash) == 0;
        return isVerified;
      }
    }
  }
}