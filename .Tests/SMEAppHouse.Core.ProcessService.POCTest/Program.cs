using System;
using System.Threading;

namespace SMEAppHouse.Core.ProcessService.POCTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var timeOut = new TimeSpan(0, 0, 0, 0, 123);
            var procViaTask = new ProcAgentViaTaskPOCInstance(timeOut)
            {
                AutoActivate = false,
                AutoStart = false
            };
            var tickTog = 1;

            procViaTask.OnReadyState += (s, e) =>
            {
                Console.WriteLine($"\nProcess engine ready!\r\n");
            };

            procViaTask.OnEngineStatusChanged += (s, e) =>
            {
                Console.WriteLine($"\nProcess engine event: {e.EngineStatus}\r\n");
            };

            procViaTask.OnTick += (s, e) =>
            {
                switch (tickTog)
                {
                    case 1:
                        Console.Write("\b\b\\");
                        break;
                    case 2:
                        Console.Write("\b|");
                        break;
                    case 3:
                        Console.Write("\b/");
                        break;
                    case 4:
                        Console.Write("\b-");
                        tickTog = 0;
                        break;
                }
                tickTog++;
            };

            var thread = new Thread(() =>
            {
                var escaped = false;
                Console.WriteLine("Press any of the below function keys to test the process engine:\nF1-Initialize\nF2-Resume\nF3-Suspend\nF4-Suspend (resume in 10 seconds)\nF5-Shutdown\nPress ESC to stop\n\n");

                while (!escaped)
                {
                    //while (!Console.KeyAvailable)
                    while (true)
                    {
                        var consKyInf = Console.ReadKey(true);

                        if (consKyInf.Key == ConsoleKey.F1)
                            procViaTask.Activate();
                        if (consKyInf.Key == ConsoleKey.F2)
                            procViaTask.Resume();
                        else if (consKyInf.Key == ConsoleKey.F3)
                            procViaTask.Suspend();
                        else if (consKyInf.Key == ConsoleKey.F4)
                            procViaTask.Suspend(new TimeSpan(0, 0, 10));
                        else if (consKyInf.Key == ConsoleKey.F5)
                            procViaTask.Shutdown();
                        else if (consKyInf.Key == ConsoleKey.Escape)
                        {
                            escaped = true;
                            procViaTask.Shutdown();
                            break;
                        }
                        Thread.Sleep(1);
                    }
                } 
            });
            thread.Start();
        }


    }
}
