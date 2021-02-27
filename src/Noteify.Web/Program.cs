using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Noteify.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        private static bool IsDevelopment =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public static string HostPort =>
            IsDevelopment
                ? "5000"
                : Environment.GetEnvironmentVariable("PORT");

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // Adds user secrets as well
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT");

                    if (!IsDevelopment)
                    {
                        // Heroku takes care of the HTTPS port!
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls($"http://*:{HostPort}");
                    }
                    else
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("https://*:5001", $"http://*:{HostPort}");
                    }
                });
    }
}
