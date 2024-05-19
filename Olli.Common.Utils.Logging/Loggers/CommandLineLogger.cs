using System;

namespace Olli.Common.Utils.Logging
{
    /// <summary>
    /// Console logger that prints logs to stdout
    /// </summary>
    public class CommandLineLogger : ALogger
    {
        private object _lock = new object();        
        private ConsoleColor previousColor;

        ///<inheritdoc />
        public override void Log(LType logType, string valueString)
        {
            lock (_lock)
            {
                previousColor = Console.ForegroundColor;  

                switch (logType)
                {
                    case LType.Exception:
                        oneOrMoreErrorsLogged = true;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(valueString);
                        break;
                    case LType.Error:
                        oneOrMoreErrorsLogged = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[Error] {valueString}");
                        break;
                    case LType.Warning:
                        oneOrMoreWarningsLogged = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[Warning] {valueString}");
                        break;
                    case LType.Info:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(valueString);
                        break;
                    case LType.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"[Debug] {valueString}");
                        break;
                    case LType.Command:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[Command] {valueString}");
                        break;
                }

                Console.ForegroundColor = previousColor;
            }
        }
    }
}
