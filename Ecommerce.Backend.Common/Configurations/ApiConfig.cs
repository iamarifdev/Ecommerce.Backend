namespace Ecommerce.Backend.Common.Configurations
{
  public class ApiConfig : IApiConfig
  {
    public string StorageBaseAddress { get; set; }
  }

  public interface IApiConfig
  {
    string StorageBaseAddress { get; set; }
  }
}