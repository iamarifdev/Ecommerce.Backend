using System;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.PaymentGateway.SSLCommerz.Models
{
  public class IPN
  {
    [FromForm(Name = "amount")]
    public decimal Amount { get; set; }

    [FromForm(Name = "bank_tran_id")]
    public string BankTransactionId { get; set; }

    [FromForm(Name = "card_brand")]
    public string CardBrand { get; set; }

    [FromForm(Name = "card_issuer")]
    public string CardIssuer { get; set; }

    [FromForm(Name = "card_issuer_country")]
    public string CardIssuerCountry { get; set; }

    [FromForm(Name = "card_issuer_country_code")]
    public string CardIssuerCountryCode { get; set; }

    [FromForm(Name = "card_no")]
    public string CardNo { get; set; }

    [FromForm(Name = "card_type")]
    public string CardType { get; set; }

    [FromForm(Name = "status")]
    public string Status { get; set; }

    [FromForm(Name = "store_amount")]
    public decimal StoreAmount { get; set; }

    [FromForm(Name = "store_id")]
    public string StoreId { get; set; }

    [FromForm(Name = "tran_date")]
    public DateTime TransactionDate { get; set; }

    [FromForm(Name = "tran_id")]
    public string TransactionId { get; set; }

    [FromForm(Name = "val_id")]
    public string ValueId { get; set; }

    [FromForm(Name = "verify_sign")]
    public string VerifySign { get; set; }

    [FromForm(Name = "verify_key")]
    public string VerifyKey { get; set; }
  }
}