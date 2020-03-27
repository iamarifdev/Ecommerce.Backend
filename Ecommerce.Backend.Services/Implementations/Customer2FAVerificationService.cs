using System;
using System.Threading.Tasks;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class Customer2FAVerificationService : BaseService<Customer2FAVerification>, ICustomer2FAVerificationService
  {
    public string GenerateVerificationCode()
    {
      var randomGenerator = new Random();
      var verficationCode = randomGenerator.Next(0, 999999).ToString("D6");
      return verficationCode;
    }

    public async Task<bool> CheckVerficationCode(string phoneNo, string verificationCode)
    {
      var isExist = await IsExist(code =>
        code.PhoneNo == phoneNo &&
        code.VerficationCode == verificationCode &&
        code.ExpiresAt >= DateTime.Now && 
        !code.IsVerified &&
        !code.IsDeleted
      );
      return isExist;
    }

    public async Task<bool> VerifyCode(string phoneNo, string verificationCode)
    {
      try
      {
        await DB.Update<Customer2FAVerification>()
          .Match(code =>
            code.PhoneNo == phoneNo &&
            code.VerficationCode == verificationCode &&
            !code.IsVerified &&
            !code.IsDeleted
          )
          .Modify(a => a.IsVerified, true)
          .Modify(a => a.UpdatedAt, DateTime.Now)
          .ExecuteAsync();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public async Task<bool> InvalidateAllVerificationCode(string phoneNo)
    {
      try
      {
        await DB.Update<Customer2FAVerification>()
          .Match(code => code.PhoneNo == phoneNo && !code.IsDeleted)
          .Modify(a => a.IsDeleted, true)
          .Modify(a => a.IsEnabled, false)
          .Modify(a => a.UpdatedAt, DateTime.Now)
          .ExecuteAsync();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
  }
}