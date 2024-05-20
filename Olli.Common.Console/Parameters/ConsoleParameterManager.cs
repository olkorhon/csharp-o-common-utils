using System;
using Olli.Common.Logging;

namespace Olli.Common.Console.Parameters
{
    public class ConsoleParameterManager : ParameterManager
    {
        /// <summary>
        /// Constructor registers the help parameter automatically
        /// </summary>
        public ConsoleParameterManager(ILogger logger) : base(logger)
        {
            AddParameter(false, "Prints the help screen.", "help", "h");
            AddParameter(false, "Prints the current command line parameter values.", "values");
        }

        /// <summary>
        /// Writes possible errors directly in command line output without raising errors.
        /// </summary>
        /// <param name="args"></param>
        public new bool Parse(string[] args)
        {
            bool ret = false;
            string error = string.Empty;

            try
            {
                ret = base.Parse(args);
            }
            catch (ArgumentException ex)
            {
                error = ex.Message;
            }

            if (this["help"].Exists)
            {
                logger.LogInfo(HelpScreen(false));
                Environment.ExitCode = 0;
            }

            if (this["values"].Exists)
            {
                logger.LogInfo(HelpScreen(false));
                Environment.ExitCode = 0;
            }

            if (string.IsNullOrWhiteSpace(error) == false)
            {
                logger.LogInfo(HelpScreen(true));
                logger.LogInfo("COMMAND LINE ARGUMENT PARSING FAILED:");
                logger.LogInfo(error);

                Environment.ExitCode = 1; //invalid function
            }

            return ret;
        }
    }
}
