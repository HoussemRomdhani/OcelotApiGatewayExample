using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ocelot.gateway.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                           .ConfigureAppConfiguration((builderContext, config) => {
                                                             config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                                                                   .AddJsonFile("appsettings.json", true, true)
                                                                   .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", true, true)
                                                                   .AddJsonFile("ocelot.json", true, true); 
                           });
        }
    }


}
