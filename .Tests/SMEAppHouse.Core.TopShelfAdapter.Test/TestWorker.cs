using System;
using System.Timers;
using Topshelf;

namespace SMEAppHouse.Core.TopShelfAdapter.Test
{
    public class TestWorker : ServiceControl
    {
        private Timer _timer = new Timer();

        public bool Start(HostControl hostControl)
        {
            _timer.Interval = 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            Console.WriteLine("Service started.");
            return true;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Service ran.");
        }

        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("Service stopped.");
            return true;
        }
    }
}
