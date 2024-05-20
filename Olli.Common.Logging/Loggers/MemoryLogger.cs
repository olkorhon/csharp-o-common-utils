using System;
using System.Collections.ObjectModel;

namespace Olli.Common.Logging
{
    /// <summary>
    /// Logger that memorizes any received logs for later processing
    /// </summary>
    public class MemoryLogger : ALogger
    {
        readonly Collection<LogEntry> logEntries;

        /// <summary>
        /// Constructor for MemoryLogger, initializes log
        /// entry collection
        /// </summary>
        public MemoryLogger()
        {
            logEntries = new Collection<LogEntry>();
        }

        /// <inheritdoc />
        public override void LogException(Exception e)
        {
            Log(new LogEntry(LType.Exception, UnpackException(e)));
        }

        /// <inheritdoc />
        public override void LogException(string exceptionString)
        {
            Log(new LogEntry(LType.Exception, exceptionString));
        }

        /// <inheritdoc />
        public override void LogError(string errorString)
        {
            Log(new LogEntry(LType.Error, errorString));
        }

        /// <inheritdoc />
        public override void LogWarning(string warningString)
        {
            Log(new LogEntry(LType.Warning, warningString));
        }

        /// <inheritdoc />
        public override void LogInfo(string infoString)
        {
            Log(new LogEntry(LType.Info, infoString));
        }

        /// <inheritdoc />
        public override void LogDebug(string debugString)
        {
            Log(new LogEntry(LType.Debug, debugString));
        }

        /// <inheritdoc />
        public override void LogCommand(string commandString)
        {
            Log(new LogEntry(LType.Command, commandString));
        }

        /// <inheritdoc />
        public override void Log(LType logType, string msgString)
        {
            Log(new LogEntry(logType, msgString));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="logEntry"></param>
        public void Log(LogEntry logEntry)
        {
            logEntries.Add(logEntry);

            switch (logEntry.LogType)
            {
                case LType.Exception:
                    oneOrMoreErrorsLogged = true;
                    break;
                case LType.Error:
                    oneOrMoreErrorsLogged = true;
                    break;
                case LType.Warning:
                    oneOrMoreWarningsLogged = true;
                    break;
            }
        }

        /// <summary>
        /// Replay all stored messages to another logger in FIFO order
        /// </summary>
        /// <param name="logger">Instance that inherits functionality from ILogger</param>
        public void ReplayLogs(ALogger logger)
        {
            foreach (LogEntry logEntry in logEntries)
                logger.Log(logEntry.LogType, logEntry.Msg);
        }
    }
}
