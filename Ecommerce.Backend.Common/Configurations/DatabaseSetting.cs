using MongoDB.Driver;

namespace Ecommerce.Backend.Common.Configurations
{
  public class DatabaseSetting : IDatabaseSetting
  {
    public string Env { get; set; }
    public string ConnectionStringTemplate { get; set; }
    public string DatabaseName { get; set; }
    public string ConnectionString => ConnectionStringTemplate.Replace("<database>", DatabaseName);
    public MongoClientSettings DbSettings => MongoClientSettings.FromConnectionString(ConnectionString);
  }

  public interface IDatabaseSetting
  {
    string Env { get; set; }
    string ConnectionStringTemplate { get; set; }
    string ConnectionString { get; }
    string DatabaseName { get; set; }
    MongoClientSettings DbSettings { get; }
  }
}