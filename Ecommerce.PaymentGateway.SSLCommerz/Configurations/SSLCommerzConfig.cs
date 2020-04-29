using System.Collections.Generic;

namespace Ecommerce.PaymentGateway.SSLCommerz.Configurations
{
  public interface ISSLCommerzConfig
  {
    string StoreName { get; set; }
    string StoreId { get; set; }
    string StoreSecretKey { get; set; }
    string BaseUrl { get; set; }
    Merchant Merchant { get; set; }
    string SubmitUrl { get; set; }
    string ValidationUrl { get; set; }
    string CheckingUrl { get; set; }
    string IPNListnerUrl { get; set; }
    API API { get; set; }
    APP APP { get; set; }
  }

  public class Merchant
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
  }

  public class API
  {
    public string BaseUrl { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
    public string FailUrl { get; set; }

    private string _buildQueryString(List<KeyValuePair<string, string>> parameters)
    {
      var queryString = "?";
      parameters.ForEach(keyValue => queryString += $"{keyValue.Key}={keyValue.Value}&");
      queryString = queryString.TrimEnd('&');
      return queryString;
    }

    private string _getUrl(List<KeyValuePair<string, string>> parameters, string urlPart)
    {
      if (parameters == null)
      {
        return $"{BaseUrl}{urlPart}";
      }
      var queryString = _buildQueryString(parameters);
      return $"{BaseUrl}{urlPart}{queryString}";
    }

    public string GetSuccessUrl(List<KeyValuePair<string, string>> parameters = null) => _getUrl(parameters, SuccessUrl);
    public string GetCancelUrl(List<KeyValuePair<string, string>> parameters = null) => _getUrl(parameters, CancelUrl);
    public string GetFailUrl(List<KeyValuePair<string, string>> parameters = null) => _getUrl(parameters, FailUrl);
  }

  public class APP : API { }

  public class SSLCommerzConfig : ISSLCommerzConfig
  {
    public string StoreName { get; set; }
    public string StoreId { get; set; }
    public string StoreSecretKey { get; set; }
    public string BaseUrl { get; set; }
    public Merchant Merchant { get; set; }
    public string SubmitUrl { get; set; }
    public string ValidationUrl { get; set; }
    public string CheckingUrl { get; set; }
    public string IPNListnerUrl { get; set; }
    public API API { get; set; }
    public APP APP { get; set; }
  }
}