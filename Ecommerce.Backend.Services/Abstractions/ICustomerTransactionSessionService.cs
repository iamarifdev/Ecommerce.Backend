using System.Threading.Tasks;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.Services.Abstractions
{
  public interface ICustomerTransactionSessionService : IBaseService<CustomerTransactionSession>
  {
    Task<CustomerTransactionSession> AddSession(string customerId, string transactionId, string currency, string sessionKey);
    Task<CustomerTransactionSession> GetSession(string sessionKey);
    Task<CustomerTransactionSession> GetSessionByCustomerId(string customerId);
  }
}