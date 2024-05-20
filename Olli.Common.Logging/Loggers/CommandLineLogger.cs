using System;

namespace Olli.Common.Logging
{
    /// <summary>
    /// Console logger that prints logs to stdout
    /// </summary>
    public class CommandLineLogger : ALogger
    {
        private readonly object _lock = new object();
        private ConsoleColor previousColor;

        ///<inheritdoc />
        public override void Log(LType logType, string msgString)
        {
            lock (_lock)
            {
                previousColor = Console.ForegroundColor;

                switch (logType)
                {
                    case LType.Exception:
                        oneOrMoreErrorsLogged = true;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(msgString);
                        break;
                    case LType.Error:
                        oneOrMoreErrorsLogged = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[Error] {msgString}");
                        break;
                    case LType.Warning:
                        oneOrMoreWarningsLogged = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[Warning] {msgString}");
                        break;
                    case LType.Info:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(msgString);
                        break;
                    case LType.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"[Debug] {msgString}");
                        break;
                    case LType.Command:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[Command] {msgString}");
                        break;
                }

                Console.ForegroundColor = previousColor;
            }
        }
    }
}
