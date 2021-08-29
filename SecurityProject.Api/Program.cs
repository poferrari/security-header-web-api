using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SecurityProject.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// https://karthiktechblog.com/aspnetcore/how-to-remove-the-server-header-from-asp-net-core-3-1
        /// </summary>        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    // Remove X-Powered-By and Server Headers
                    // It is not recommended to leak the server type and version number (i.e. ASP.NET, Kestrel, IIS) to an anonymous client
                    webBuilder.UseKestrel((options) =>
                    {
                        // Do not add the Server HTTP header.
                        options.AddServerHeader = false;
                    });
                });
    }
}
