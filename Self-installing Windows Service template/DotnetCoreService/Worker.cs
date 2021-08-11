using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace DotnetCoreService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var sw = new StreamWriter(@"C:\Temp\testsvc.txt", append: true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + " args: " + String.Join(" ", Program.Args));
                }
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
