using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace Ecommerce.Backend.API
{
#pragma warning disable CS1591
#pragma warning disable CS1573
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args)
        .Build()
        .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(webBuilder =>
      {
        webBuilder.UseStartup<Startup>();
      });
  }
#pragma warning restore CS1591
#pragma warning disable CS1573
}