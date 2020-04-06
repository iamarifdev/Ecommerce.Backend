using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ecommerce.PaymentGateway.SSLCommerz.Models
{
  public class InitResponse
  {
    [JsonPropertyName("status")]
    public string status { get; set; }

    [JsonPropertyName("failedreason")]
    public string FailedReason { get; set; }

    [JsonPropertyName("sessionkey")]
    public string SessionKey { get; set; }

    [JsonPropertyName("gw")]
    public Gateway Gateway { get; set; }

    [JsonPropertyName("redirectGatewayURL")]
    public string RedirectGatewayURL { get; set; }

    [JsonPropertyName("redirectGatewayURLFailed")]
    public string RedirectGatewayURLFailed { get; set; }

    [JsonPropertyName("GatewayPageURL")]
    public string GatewayPageURL { get; set; }

    [JsonPropertyName("storeBanner")]
    public string StoreBanner { get; set; }

    [JsonPropertyName("storeLogo")]
    public string StoreLogo { get; set; }

    [JsonPropertyName("desc")]
    public IEnumerable<Description> Descriptions { get; set; }

    [JsonPropertyName("is_direct_pay_enable")]
    public int IsDirectPayEnable { get; set; }
  }
}