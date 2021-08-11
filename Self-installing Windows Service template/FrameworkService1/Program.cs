using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkService1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "run")
                {
                    Console.WriteLine("Running service, stop by pressing return");
                    var svc = new Service1();
                    svc.DoStart();
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                else if (args[0].ToLower() == "install")
                {
                    Console.WriteLine("Installing service");
                    SelfInstaller.InstallMe();
                    Environment.Exit(0);
                }
                else if (args[0].ToLower() == "uninstall")
                {
                    Console.WriteLine("Uninstalling service");
                    SelfInstaller.UninstallMe();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Unknown command");
                    Environment.Exit(1);
                }
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1() { }
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
