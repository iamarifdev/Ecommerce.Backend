using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Ecommerce.PaymentGateway.SSLCommerz.Configurations;

namespace Ecommerce.PaymentGateway.SSLCommerz.Helpers
{
  public class SSLCommerzHttpClient
  {
    public HttpClient Http { get; set; }

    public SSLCommerzHttpClient(ISSLCommerzConfig config)
    {
      var httpClientHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
      };

      Http = new HttpClient(httpClientHandler)
      {
        BaseAddress = new Uri(config.BaseUrl)
      };
      Http.DefaultRequestHeaders.Accept.Clear();
      Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
  }
}