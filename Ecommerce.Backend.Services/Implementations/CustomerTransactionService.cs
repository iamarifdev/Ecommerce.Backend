using System.Threading.Tasks;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Ecommerce.PaymentGateway.SSLCommerz.Models;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CustomerTransactionService : BaseService<CustomerTransaction>, ICustomerTransactionService
  {
    public async Task<CustomerTransaction> AddTransaction(CustomerTransactionSession session, IPN ipn)
    {
      var customerTransaction = new CustomerTransaction
      {
        Amount = ipn.Amount,
        BankTransactionId = ipn.BankTransactionId,
        CardNo = ipn.CardNo,
        CardType = ipn.CardType,
        Currency = session.Currency,
        CustomerRef = session.CustomerRef,
        SessionKey = session.SessionKey,
        Status = ipn.Status, // need to check
        StoreAmount = ipn.StoreAmount,
        StoreId = ipn.StoreId,
        TransactionDate = ipn.TransactionDate,
        TransactionId = ipn.TransactionId
      };
      await Add(customerTransaction);
      return customerTransaction;
    }

    public async Task<bool> IsTransactionExist(string sessionKey, string transactionId)
    {
      var isExist = await IsExist(session => 
        session.SessionKey == sessionKey && 
        session.TransactionId == transactionId && 
        session.IsEnabled
      );
      return isExist;
    }
  }
}