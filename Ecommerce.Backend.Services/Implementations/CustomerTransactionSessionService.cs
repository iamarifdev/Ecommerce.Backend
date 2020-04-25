using System;
using System.Threading.Tasks;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using MongoDB.Entities;

namespace Ecommerce.Backend.Services.Implementations
{
  public class CustomerTransactionSessionService : BaseService<CustomerTransactionSession>, ICustomerTransactionSessionService
  {
    private readonly ICartService _cartService;
    public CustomerTransactionSessionService(ICartService cartService)
    {
      _cartService = cartService;
    }
    public async Task<CustomerTransactionSession> AddSession(string customerId, string transactionId, string currency, string sessionKey)
    {
      var cart = await _cartService.GetCartById(customerId: customerId);
      if (cart == null) return null;
      var transactionSession = new CustomerTransactionSession
      {
        CustomerRef = new One<Customer> { ID = customerId },
        TransactionId = transactionId,
        CartRef = new One<Cart> { ID = cart.ID },
        Amount = cart.TotalPrice,
        Currency = currency,
        SessionKey = sessionKey
      };
      return await Add(transactionSession);
    }

    public async Task<CustomerTransactionSession> GetSession(string sessionKey)
    {
      var transactionSession = await GetByExpression(session => session.SessionKey == sessionKey && session.IsEnabled);
      return transactionSession;
    }

    public async Task<CustomerTransactionSession> GetSessionByCustomerId(string customerId)
    {
      var transactionSession = await GetByExpression(session => session.CustomerRef.ID == customerId && session.IsEnabled);
      return transactionSession;
    }
  }
}