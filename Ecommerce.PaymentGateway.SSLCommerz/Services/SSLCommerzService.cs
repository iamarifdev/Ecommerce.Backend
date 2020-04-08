using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ecommerce.PaymentGateway.SSLCommerz.Configurations;
using Ecommerce.PaymentGateway.SSLCommerz.Helpers;
using Ecommerce.PaymentGateway.SSLCommerz.Models;
using MongoDB.Bson;

namespace Ecommerce.PaymentGateway.SSLCommerz.Services
{

  public class SSLCommerzService: ISSLCommerzService
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

    private Dictionary<string, string> _prepareRequestParameters(Dictionary<string, string> parameters)
    {
      // Integration Required Parameters
      parameters.Add("store_id", _config.StoreId);
      parameters.Add("store_passwd", _config.StoreSecretKey);
      parameters.Add("total_amount", "500.00");
      parameters.Add("currency", Currency.BDT);
      parameters.Add("tran_id", ObjectId.GenerateNewId().ToString());
      parameters.Add("success_url", _config.SuccessUrl);
      parameters.Add("fail_url", _config.FailUrl);
      parameters.Add("cancel_url", _config.CancelUrl);
      parameters.Add("ipn_url", _config.IPNListnerUrl);
      parameters.Add("emi_option", $"{EMIOption.Enabled}");

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
      parameters.Add("shipping_method", "YES"); // Example: YES or NO or Courier
      parameters.Add("num_of_item", "5");
      parameters.Add("ship_name", "H#04, R#22, Block-D, Mirpur-12");
      parameters.Add("ship_add1", "H#04, R#22, Block-D, Mirpur-12");
      parameters.Add("ship_city", "Dhaka");
      parameters.Add("ship_state", "Dhaka");
      parameters.Add("ship_postcode", "1216");
      parameters.Add("ship_country", "Bangladesh");

      // Product Information
      parameters.Add("product_name", "Test product");
      parameters.Add("product_category", "Shoes");
      parameters.Add("product_profile", ProductProfile.PhysicalGoods);
      parameters.Add("cart", "[{\"product\":\"DHK TO BRS AC A1\",\"quantity\":\"1\",\"amount\":\"200.00\"},{\"product\":\"DHK TO BRS AC A2\",\"quantity\":\"1\",\"amount\":\"200.00\"},{\"product\":\"DHK TO BRS AC A3\",\"quantity\":\"1\",\"amount\":\"200.00\"},{\"product\":\"DHK TO BRS AC A4\",\"quantity\":\"2\",\"amount\":\"200.00\"}]");
      return parameters;
    }

    public async Task<InitResponse> InitiateTransaction(Dictionary<string, string> parameters)
    {
      parameters = _prepareRequestParameters(parameters);
      var urlEncodedContent = new FormUrlEncodedContent(parameters);
      var response = await _http.PostAsync($"{_config.SubmitUrl}", urlEncodedContent);
      var initResponse = await response.Content.ReadAsJsonAsync<InitResponse>();
      return initResponse;
    }
  }
}