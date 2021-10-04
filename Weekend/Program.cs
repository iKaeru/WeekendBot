using System;
using Weekend.Loggers;
using Weekend.Workers;

namespace Weekend
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new TelegramWorkerService(new Logger());
            worker.ExecuteCore();
        }
    }
}