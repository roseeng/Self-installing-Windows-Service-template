using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotnetCoreService
{
    public class Program
    {
        public static string[] Args;

        public static void Main(string[] args)
        {
            var svc = new ServiceController();

            svc.Name = "DotnetCoreService";
            svc.DisplayName = "DotnetCoreService";
            svc.Description = "A demo of a self-installing Windows service in dotnet Core";

            Args = args;
            string command = "";

            if (args.Length > 0)
            {
                command = args[0].ToLower();
            }

            switch (command)
            {
                case "install":
                    svc.Install();
                    break;
                case "uninstall":
                    svc.Uninstall();
                    break;
                case "start":
                    svc.Start();
                    break;
                case "stop":
                    svc.Stop();
                    break;
                case "status":
                    svc.Status();
                    break;

                // Run the worker in a normal console
                case "run":
                    CreateRunHostBuilder(args).Build().Run();
                    break;

                // No arguments or any other than the above means to start the service
                default:
                    CreateHostBuilder(args).Build().Run();
                    break;
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                }).UseWindowsService();

        public static IHostBuilder CreateRunHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                }).UseConsoleLifetime();
    }
}
