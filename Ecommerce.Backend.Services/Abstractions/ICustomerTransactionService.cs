using System.Threading.Tasks;
using Ecommerce.Backend.Entities;
using Ecommerce.PaymentGateway.SSLCommerz.Models;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface ICustomerTransactionService : IBaseService<CustomerTransaction>
  {
    Task<CustomerTransaction> AddTransaction(CustomerTransactionSession session, IPN ipn);
    Task<bool> IsTransactionExist(string sessionKey, string transactionId);
  }
}