using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace FrameworkService1
{
    public partial class Service1 : ServiceBase
    {
        Timer _timer;
        const double seconds = 1000.0;
        public Service1()
        {
            InitializeComponent();
            _timer = new Timer(2*seconds);
            _timer.AutoReset = true;
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Execute();
        }

        protected override void OnStart(string[] args)
        {
            DoStart();
        }

        protected override void OnStop()
        {
            DoStop();
        }

        public void DoStart()
        {
            _timer.Start();
        }

        public void DoStop()
        {
            _timer.Stop();
        }

        public void Execute()
        {
            using (var sw = new StreamWriter(@"C:\Temp\testsvc.txt"))
            {
                sw.WriteLine(DateTime.Now.ToString());
            }
        }
    }
}
