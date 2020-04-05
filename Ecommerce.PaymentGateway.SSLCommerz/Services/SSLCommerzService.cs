using System.Net.Http;
using Ecommerce.PaymentGateway.SSLCommerz.Configurations;
using Ecommerce.PaymentGateway.SSLCommerz.Helpers;

namespace Ecommerce.PaymentGateway.SSLCommerz.Services
{
  public class SSLCommerzService
  {
    private readonly ISSLCommerzConfig _config;
    private readonly SSLCommerzHttpClient _httpClient;
    private readonly HttpClient _http;
    public SSLCommerzService(SSLCommerzHttpClient httpClient, ISSLCommerzConfig config)
    {
      _httpClient = httpClient;
      _http = httpClient.Http;
      _config = config;
    }

    
  }
}