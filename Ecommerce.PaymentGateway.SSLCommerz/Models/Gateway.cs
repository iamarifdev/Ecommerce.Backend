using System.Text.Json.Serialization;

namespace Ecommerce.PaymentGateway.SSLCommerz.Models
{
  public class Gateway
  {
    [JsonPropertyName("visa")]
    public string VISA { get; set; }

    [JsonPropertyName("master")]
    public string MasterCard { get; set; }

    [JsonPropertyName("amex")]
    public string AMEX { get; set; }

    [JsonPropertyName("othercards")]
    public string OtherCards { get; set; }

    [JsonPropertyName("internetbanking")]
    public string InternetBanking { get; set; }

    [JsonPropertyName("mobilebanking")]
    public string MobileBanking { get; set; }
  }
}