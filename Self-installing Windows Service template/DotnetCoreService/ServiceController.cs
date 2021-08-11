using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCoreService
{
    public enum StartupType
    {
        boot,
        system,
        auto,
        demand, // "Manual"
        disabled,
        delayed_auto
    }

    public class ServiceController
    {
        //        public string ExePath { get; } = Assembly.GetExecutingAssembly().Location;
        public string ExePath { get; } = Process.GetCurrentProcess().MainModule.FileName;

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }


        public StartupType StartupType { get; set; } = StartupType.demand;
        public string LogonAs { get; set; } = "LocalSystem";

        public ServiceController()
        {
        }

        private ProcessStartInfo GetStartInfo()
        {
            return new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "sc.exe"
            };
        }

        public void Query()
        {
            RunSc("query " + Name);
        }

        public void Start()
        {
            RunSc("start " + Name);
        }

        public void Stop()
        {
            RunSc("stop " + Name);
        }

        public void Status()
        {
            RunSc("queryex " + Name);
        }

        public void Install()
        {
            var exetype = Path.GetExtension(this.ExePath);
            if (exetype.ToLower() != ".exe")
                throw new ApplicationException($"The executing assembly is not an exe ({exetype}), have you created a published version?");

            var command = "create " +
                this.Name + " " +
                $"DisplayName={this.DisplayName} " +
                $"binPath=\"{this.ExePath}\" " +
                $"start={this.StartupType} " +
                $"obj={this.LogonAs}";

            RunSc(command);

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                command = "description " + this.Name + " \"" + this.Description + "\"";
                RunSc(command);
            }
        }

        public void Uninstall()
        {
            RunSc("delete " + Name);
        }

        private void RunSc(string arguments)
        {
            var startInfo = GetStartInfo();
            startInfo.Arguments = arguments;

            Console.WriteLine("$ sc " + arguments);
            using (var proc = Process.Start(startInfo))
            {
                var colour = Console.ForegroundColor;

                proc.WaitForExit();
                if (proc.ExitCode != 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(proc.StandardOutput.ReadToEnd());
                Console.ForegroundColor = colour;
            }
        }
    }

    /*
    https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create

    DESCRIPTION:
            Creates a service entry in the registry and Service Database.
    USAGE:
            sc <server> create [service name] [binPath= ] <option1> <option2>...

    OPTIONS:
    NOTE: The option name includes the equal sign.
          A space is required between the equal sign and the value.
     type= <own|share|interact|kernel|filesys|rec|userown|usershare>
           (default = own)
     start= <boot|system|auto|demand|disabled|delayed-auto>
           (default = demand)
     error= <normal|severe|critical|ignore>
           (default = normal)
     binPath= <BinaryPathName to the .exe file>
     group= <LoadOrderGroup>
     tag= <yes|no>
     depend= <Dependencies(separated by / (forward slash))>
     obj= <AccountName|ObjectName>
           (default = LocalSystem)
     DisplayName= <display name>
     password= <password>
    */
}
