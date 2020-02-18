namespace Ecommerce.Backend.Common.Configurations
{
  public class AppConfig : IAppConfig
  {
    public string ProductFeatureImagesDirectory { get; set; }
    public string ProductImagesDirectory { get; set; }
    public string ProductDirectory { get; set; }
  }

  public interface IAppConfig
  {
    string ProductFeatureImagesDirectory { get; set; }
    string ProductImagesDirectory { get; set; }
    string ProductDirectory { get; set; }
  }
}