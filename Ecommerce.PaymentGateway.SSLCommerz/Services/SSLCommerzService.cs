using System.Collections.Generic;
using System.Net.Http;
using Ecommerce.PaymentGateway.SSLCommerz.Configurations;
using Ecommerce.PaymentGateway.SSLCommerz.Helpers;
using Ecommerce.PaymentGateway.SSLCommerz.Models;
using MongoDB.Bson;

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

    public Dictionary<string, object> PrepareRequestParameters(Dictionary<string, object> parameters)
    {
      // Integration Required Parameters
      parameters.Add("store_id", _config.StoreId);
      parameters.Add("store_passwd", _config.StoreSecretKey);
      parameters.Add("currency", Currency.BDT);
      parameters.Add("tran_id", ObjectId.GenerateNewId().ToString());
      parameters.Add("success_url", _config.SuccessUrl);
      parameters.Add("fail_url", _config.FailUrl);
      parameters.Add("cancel_url", _config.CancelUrl);
      parameters.Add("ipn_url", _config.IPNListnerUrl);
      parameters.Add("emi_option", EMIOption.Enabled);
      // it will eventually come from frontend
      parameters.Add("product_category", "clothing");

      // Customer Information
      parameters.Add("cus_name", "Test customer");
      parameters.Add("cus_email", "ariful+test@binate-solutions.com");
      parameters.Add("cus_add1", "H#04, R#22, Block-D, Mirpur-12");
      parameters.Add("cus_city", "Dhaka");
      parameters.Add("cus_state", "Dhaka");
      parameters.Add("cus_postcode", "1216");
      parameters.Add("cus_country", "Bangladesh");
      parameters.Add("cus_phone", "01793574440");
      
      // Shipment Information
      parameters.Add("shipping_method", "Courier"); // Example: YES or NO or Courier
      parameters.Add("num_of_item", 5);

      // Product Information
      parameters.Add("product_name", "Test product");
      parameters.Add("product_category", "Shoes");
      parameters.Add("product_profile", ProductProfile.PhysicalGoods);
      return parameters;
    }

    
  }
}