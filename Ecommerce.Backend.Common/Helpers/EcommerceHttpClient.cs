using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Ecommerce.Backend.Common.Configurations;

namespace Ecommerce.Backend.Common.Helpers
{
  public class EcommerceHttpClient
  {
    public HttpClient Http { get; set; }

    public EcommerceHttpClient(IApiConfig config)
    {
      var httpClientHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
      };

      Http = new HttpClient(httpClientHandler)
      {
        BaseAddress = new Uri(config.StorageBaseAddress)
      };
      Http.DefaultRequestHeaders.Accept.Clear();
      Http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
  }
}