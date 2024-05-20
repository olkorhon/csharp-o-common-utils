using System;
using Olli.Common.Logging;

namespace Olli.Common.LoggingTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileLogger logger = new FileLogger(@".\testlog.txt");

            string input = "";
            while (!input.Trim().Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                input = Console.ReadLine();
                logger.LogInfo(input);
            }
        }
    }
}
