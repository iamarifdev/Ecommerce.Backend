using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.PaymentGateway.SSLCommerz.Models;

namespace Ecommerce.PaymentGateway.SSLCommerz.Services
{
  public interface ISSLCommerzService
  {
    Task<InitResponse> InitiateTransaction(Dictionary<string, string> parameters);
  }
}