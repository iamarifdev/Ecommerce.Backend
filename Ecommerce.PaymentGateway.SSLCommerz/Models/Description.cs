using System.Text.Json.Serialization;

namespace Ecommerce.PaymentGateway.SSLCommerz.Models
{
  public class Description
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("logo")]
    public string Logo { get; set; }

    [JsonPropertyName("gw")]
    public string Gateway { get; set; }
  }
}