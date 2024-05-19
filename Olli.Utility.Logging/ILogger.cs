using System;

namespace Olli.Utility.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Allows logger user to know if errors have been logged during logger instances lifetime
        /// </summary>
        /// <returns>true if atleast one error has been logged</returns>
        bool OneOrMoreErrors { get; }

        /// <summary>
        /// Allows logger user to know if warnings have been logged during logger instances lifetime
        /// </summary>
        /// <returns>true if atleast one warning has been logged</returns>
        bool OneOrMoreWarnings { get; }

        /// <summary>
        /// Unpacks exception and related inner exceptions and writes result to log endpoint
        /// </summary>
        /// <param name="e">Exception instance</param>
        void LogException(Exception e);

        /// <summary>
        /// Writes text to log endpoint, used when custom formatting for exception is preferred
        /// </summary>
        /// <param name="exceptionString">Exception text</param>
        void LogException(string exceptionString);

        /// <summary>
        /// Writes text to log endpoint, used when something fatal happens and program cannot continue.
        /// Errors are often highlighted in log readers.
        /// </summary>
        /// <param name="errorString">Error text</param>
        void LogError(string errorString);

        /// <summary>
        /// Writes text to log endpoint, used when something odd but not fatal happens.
        /// Warnings are often highlighted in log readers.
        /// </summary>
        /// <param name="warningString">Warning text</param>
        void LogWarning(string warningString);

        /// <summary>
        /// Writes text to log endpoint, usually status information about the progress of ongoing task
        /// </summary>
        /// <param name="infoString">Info text</param>
        void LogInfo(string infoString);

        /// <summary>
        ///  Writes text to log endpoint, usually needlessly detailed information that is potentially useful for debugging
        /// </summary>
        /// <param name="debugString">Debug text</param>
        void LogDebug(string debugString);

        /// <summary>
        /// Writes text to log endpoint, intended for highlighting raw commandline commands
        /// </summary>
        /// <param name="commandString">Command text</param>
        void LogCommand(string commandString);

        /// <summary>
        /// Writes text to log endpoint, provides only the basic formatting linked to provided log type
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="valueString"></param>
        void Log(LType logType, string valueString);
    }
}
