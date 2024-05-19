using System;
using System.Text;

namespace Olli.Common.Utils.Logging
{
    /// <summary>
    /// Common functionality and interface definition for all loggers
    /// </summary>
    public abstract class ALogger : ILogger
    {
        protected bool oneOrMoreErrorsLogged;
        protected bool oneOrMoreWarningsLogged;

        protected ALogger()
        {
            oneOrMoreErrorsLogged = false;
            oneOrMoreWarningsLogged = false;
        }

        /// <inheritdoc/>
        public bool OneOrMoreErrors { get { return oneOrMoreErrorsLogged; } }

        /// <inheritdoc/>
        public bool OneOrMoreWarnings { get { return oneOrMoreWarningsLogged; } }

        /// <inheritdoc/>
        public virtual void LogException(Exception e)
        {
            Log(LType.Exception, UnpackException(e));
        }

        /// <inheritdoc/>
        public virtual void LogException(string exceptionString)
        {
            Log(LType.Exception, exceptionString);
        }

        /// <inheritdoc/>
        public virtual void LogError(string errorString)
        {
            Log(LType.Error, errorString);
        }

        /// <inheritdoc/>
        public virtual void LogWarning(string warningString)
        {
            Log(LType.Warning, warningString);
        }

        /// <inheritdoc/>
        public virtual void LogInfo(string infoString)
        {
            Log(LType.Info, infoString);
        }

        /// <inheritdoc/>
        public virtual void LogDebug(string debugString)
        {
            Log(LType.Debug, debugString);
        }

        /// <inheritdoc/>
        public virtual void LogCommand(string commandString)
        {
            Log(LType.Command, commandString);
        }

        /// <inheritdoc/>
        public abstract void Log(LType logType, string valueString);

        /// <summary>
        /// Peels exception onion into a single multiline human readable string.
        /// Recurses inner exceptions up to 5 levels downwards.
        /// </summary>
        /// <param name="e">Exception instance</param>
        /// <returns></returns>
        protected string UnpackException(Exception e)
        {
            StringBuilder sb = new StringBuilder();

            // Exception might be null, user should know
            if (e == null)
            {
                sb.AppendLine("[Exception] INVALID exception, tried to log null exception");
                return sb.ToString();
            }

            // Peel and log exception onion
            int tries = 5;
            for (int i = 1; i <= tries && e != null; i++)
            {
                if (i == 1)
                {
                    sb.AppendLine($@"[Exception]    Message: {e.Message}{Environment.NewLine}
                                     [Exception] StackTrace: {e.StackTrace}");
                }
                else
                {
                    sb.AppendLine($@"[InnerException#{i}]    Message: {e.Message}{Environment.NewLine}
                                     [InnerException#{i}] StackTrace: {e.StackTrace}");
                }

                e = e.InnerException;
            }

            return sb.ToString();
        }
    }
}
