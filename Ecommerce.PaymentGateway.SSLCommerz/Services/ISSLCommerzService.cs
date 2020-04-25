using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.PaymentGateway.SSLCommerz.Models;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.PaymentGateway.SSLCommerz.Services
{
  public interface ISSLCommerzService
  {
    Task<InitResponse> InitiateTransaction(Dictionary<string, string> parameters);
    (bool, string) CheckIPNStatus(IFormCollection ipn);
    Task <(bool, string)> ValidateTransaction(string transactionId, decimal transactionAmount, string transactionCurrency, IFormCollection formValue);
    bool VerifyIPNHash(IFormCollection ipn);
  }
}