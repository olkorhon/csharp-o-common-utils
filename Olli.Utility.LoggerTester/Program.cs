using System;
using Olli.Utility.Logging;

namespace Olli.Utility.LoggerTester
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
