using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Ecommerce.PaymentGateway.SSLCommerz.Configurations;
using Ecommerce.PaymentGateway.SSLCommerz.Helpers;
using Ecommerce.PaymentGateway.SSLCommerz.Models;
using Microsoft.AspNetCore.Http;
namespace Ecommerce.PaymentGateway.SSLCommerz.Services
{

  public class SSLCommerzService : ISSLCommerzService
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
      parameters.Add("success_url", $"{_config.API.BaseUrl}{_config.API.SuccessUrl}");
      parameters.Add("fail_url", $"{_config.API.BaseUrl}{_config.API.FailUrl}");
      parameters.Add("cancel_url", $"{_config.API.BaseUrl}{_config.API.CancelUrl}");
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

    public (bool, string) CheckIPNStatus(IFormCollection ipn)
    {
      if (string.IsNullOrWhiteSpace(ipn["status"]))
      {
        return (false, "Invalid IPN, no status key or value is present.");
      }
      var status = ipn["status"].ToString();
      switch (status)
      {
        case Status.VALID:
          return (true, "Successfull Transaction.");
        case Status.FAILED:
          return (false, "Transaction is declined by customer's Issuer Bank.");
        case Status.CANCELLED:
          return (false, "Transaction is cancelled by the customer.");
        default:
          return (false, "Invalid IPN, no valid status is present.");
      }
    }

    /// <summary>
    /// After receiving the IPN from SSLCommerz, it should be checked againsts
    /// its key values generated MD5 hash and verify_sign
    /// </summary>
    /// <param name="formValue"></param>
    /// <returns></returns>
    public bool VerifyIPNHash(IFormCollection formValue)
    {
      var verifyKey = "verify_key";
      var verifySignKey = "verify_sign";

      if (string.IsNullOrWhiteSpace(formValue[verifyKey]) || string.IsNullOrWhiteSpace(formValue[verifySignKey]))
      {
        return false;
      }

      var keyList = formValue[verifyKey].ToString().Split(',').ToList<string>();
      var keyValues = new List<KeyValuePair<string, string>>();

      keyList.ForEach(key => keyValues.Add(new KeyValuePair<string, string>(key, formValue[key])));

      keyValues.Add(new KeyValuePair<string, string>(
        "store_passwd",
        MD5Hash.Generate(_config.StoreSecretKey)
      ));

      keyValues.Sort(
        delegate(KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
        {
          return pair1.Key.CompareTo(pair2.Key);
        }
      );

      var queryString = "";
      keyValues.ForEach(keyValue => queryString += $"{keyValue.Key}={keyValue.Value}&");
      queryString = queryString.TrimEnd('&');

      var generatedHash = MD5Hash.Generate(queryString);
      var verifySign = formValue[verifySignKey].ToString();
      var isMatched = MD5Hash.Verify(generatedHash, verifySign, true);
      return isMatched;
    }

    /// <summary>
    /// After receiving the success response from SSLCommerz,
    /// it should check IPN hash, tran_id and merchant information 
    /// </summary>
    /// <param name="formValue">formValue</param>
    /// <returns></returns>
    public async Task<(bool, string)> ValidateTransaction(string transactionId, decimal transactionAmount, string transactionCurrency, IFormCollection formValue)
    {
      var message = "";

      var isHashVerified = VerifyIPNHash(formValue);
      if (isHashVerified)
      {
        var valueIdKey = "val_id";
        var validationUrl = $"{_config.BaseUrl}{_config.ValidationUrl}?";

        var encodedValueId = HttpUtility.UrlEncode(formValue[valueIdKey]);
        var encodedStoreId = HttpUtility.UrlEncode(_config.StoreId);
        var encodedStorePassword = HttpUtility.UrlEncode(_config.StoreSecretKey);

        validationUrl += $"val_id={encodedValueId}&";
        validationUrl += $"store_id={encodedStoreId}&";
        validationUrl += $"store_passwd={encodedStorePassword}&v=1&format=json";

        var response = await _http.GetAsync(validationUrl);
        if (response == null)
        {
          message = "Unable to get Transaction status";
          return (false, message);
        }
        var validatorResponse = await response.Content.ReadAsJsonAsync<ValidatorResponse>();
        if (validatorResponse.status == Status.VALID || validatorResponse.status == Status.VALIDATED)
        {
          var isValidTransaction = false;
          if (transactionCurrency == Currency.BDT)
          {
            isValidTransaction = (
              transactionId == validatorResponse.TransactionId &&
              (Math.Abs(transactionAmount - Convert.ToDecimal(validatorResponse.Amount)) < 1)
            );
            if (isValidTransaction)
            {
              return (true, null);
            }
            else
            {
              message = "Amount not matching";
              return (false, message);
            }
          }
          else
          {
            isValidTransaction = (
              transactionId == validatorResponse.TransactionId &&
              (Math.Abs(transactionAmount - Convert.ToDecimal(validatorResponse.CurrencyAmount)) < 1) &&
              transactionCurrency == validatorResponse.CurrencyType
            );
            if (isValidTransaction)
            {
              return (true, null);
            }
            else
            {
              message = "Currency Amount not matching";
              return (false, message);
            }
          }
        }
        else
        {
          message = "This transaction is either expired or fails";
          return (false, message);
        }
      }
      else
      {
        message = "Unable to verify hash";
        return (false, message);
      }
    }

  }
}