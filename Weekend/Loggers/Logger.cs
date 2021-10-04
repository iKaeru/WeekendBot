using System;
using System.IO;
using Weekend.Models;

namespace Weekend.Loggers
{
    public class Logger
    {
        public Action<LoggingInfo> OutputWriter { get; }

        private readonly string FileAddressPath =
            string.Format("..{0}..{0}..{0}..{0}Logs.txt", Path.DirectorySeparatorChar);

        public Logger()
        {
            OutputWriter = WriteToFile;
        }

        private void WriteToFile(LoggingInfo info)
        {
            // todo
        }
    }
}