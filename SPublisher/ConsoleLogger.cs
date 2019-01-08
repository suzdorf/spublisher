using System;
using SPublisher.Core.Log;

namespace SPublisher
{
    public class ConsoleLogger : IConsoleLogger
    {
        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
        }
    }
}