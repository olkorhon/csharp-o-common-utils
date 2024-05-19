using System;
using System.Collections.ObjectModel;

namespace Olli.Common.Utils.Logging
{
    public class WpfLogger : ALogger
    {
        readonly ObservableCollection<LogEntry> logEntries;

        /// <summary>
        /// Constructor for WpfLogger, provides an observable collection of log entries
        /// </summary>
        public WpfLogger()
        {
            logEntries = new ObservableCollection<LogEntry>();
        }

        /// <summary>
        /// Observable collection containing log entries known to this logger
        /// </summary>
        public ObservableCollection<LogEntry> LogEntries
        {
            get { return logEntries; }
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
        public override void Log(LType logType, string msg)
        {
            Log(new LogEntry(logType, msg));
        }

        /// <inheritdoc />
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
        /// Remove all logs from storage
        /// </summary>
        public void Clear()
        {
            logEntries.Clear();
        }
    }
}
