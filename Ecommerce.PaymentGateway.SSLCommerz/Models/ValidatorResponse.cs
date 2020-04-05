using System.Text.Json.Serialization;

namespace Ecommerce.PaymentGateway.SSLCommerz.Models
{
  public class ValidatorResponse
  {
    [JsonPropertyName("status")]
    public string status { get; set; }

    [JsonPropertyName("tran_date")]
    public string TransactionDate { get; set; }

    [JsonPropertyName("tran_id")]
    public string TransactionId { get; set; }

    [JsonPropertyName("val_id")]
    public string ValueId { get; set; }

    [JsonPropertyName("amount")]
    public string Amount { get; set; }

    [JsonPropertyName("store_amount")]
    public string StoreAmount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("bank_tran_id")]
    public string BankTransactionId { get; set; }

    [JsonPropertyName("card_type")]
    public string CardType { get; set; }

    [JsonPropertyName("card_no")]
    public string CardNo { get; set; }

    [JsonPropertyName("card_issuer")]
    public string CardIssuer { get; set; }

    [JsonPropertyName("card_brand")]
    public string card_brand { get; set; }

    [JsonPropertyName("card_issuer_country")]
    public string CardIssuerCountry { get; set; }

    [JsonPropertyName("card_issuer_country_code")]
    public string CardIssuerCountryCode { get; set; }

    [JsonPropertyName("currency_type")]
    public string CurrencyType { get; set; }

    [JsonPropertyName("currency_amount")]
    public string CurrencyAmount { get; set; }

    [JsonPropertyName("currency_rate")]
    public string CurrencyRate { get; set; }

    [JsonPropertyName("base_fair")]
    public string BaseFair { get; set; }

    [JsonPropertyName("value_a")]
    public string ValueA { get; set; }

    [JsonPropertyName("value_b")]
    public string ValueB { get; set; }

    [JsonPropertyName("value_c")]
    public string ValueC { get; set; }

    [JsonPropertyName("value_d")]
    public string ValueD { get; set; }

    [JsonPropertyName("emi_instalment")]
    public string EMIInstalment { get; set; }

    [JsonPropertyName("emi_amount")]
    public string EMIAccount { get; set; }

    [JsonPropertyName("emi_description")]
    public string EMIDescription { get; set; }

    [JsonPropertyName("emi_issuer")]
    public string EMIIssuer { get; set; }

    [JsonPropertyName("account_details")]
    public string AccountDetails { get; set; }

    [JsonPropertyName("risk_title")]
    public string RiskTitle { get; set; }

    [JsonPropertyName("risk_level")]
    public string RiskLevel { get; set; }

    [JsonPropertyName("APIConnect")]
    public string APIConnect { get; set; }

    [JsonPropertyName("validated_on")]
    public string ValidatedOn { get; set; }

    [JsonPropertyName("gw_version")]
    public string GatewayVersion { get; set; }

    [JsonPropertyName("token_key")]
    public string TokenKey { get; set; }
  }
}