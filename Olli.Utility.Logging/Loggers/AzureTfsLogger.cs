using System;

namespace Olli.Utility.Logging
{
    /// <summary>
    /// Console logger that prints logs in Azure comptaible way, errors and exceptions autoamtically Fail the ongoing build
    /// </summary>
    public class AzureTfsLogger : ALogger
    {
        private object _lock = new object();

        /// <summary>
        /// Collection of Azure pipeline issue types. Indicates issue severity, error, warning, etc.
        /// </summary>
        public enum IssueType { Error, Warning };

        /// <summary>
        /// Collection of Azure pipeline task results. Indicates success, failure, etc.
        /// </summary>
        public enum TaskResult { Succeeded, SucceededWithIssues, Failed }

        /// <summary>
        /// Logs issue message that will be visible in pipeline run details, can be either error or warning
        /// </summary>
        /// <param name="issueType">Azure pipeline issue type</param>
        /// <param name="description">Message describing the issue</param>
        public void LogIssue(IssueType issueType, string description)
        {
            switch (issueType)
            {
                case IssueType.Error:
                    Console.WriteLine($"##vso[task.logissue type=error;]{description}");
                    break;
                case IssueType.Warning:
                    Console.WriteLine($"##vso[task.logissue type=warning;]{description}");
                    break;
            }
        }

        /// <summary>
        /// Writes task result based on logged errors and warnings:
        /// Failed if errors logged, SucceededWithIssues if warnings logged, Succeeded otherwise
        /// </summary>
        public void WriteTaskResult()
        {
            if (OneOrMoreErrors) WriteTaskResult(TaskResult.Failed);
            else if (OneOrMoreWarnings) WriteTaskResult(TaskResult.SucceededWithIssues);
            else WriteTaskResult(TaskResult.Succeeded);
        }

        /// <summary>
        /// Set task complete with provided result and matching message
        /// </summary>
        /// <param name="result">Azure pipeline task result</param>
        public void WriteTaskResult(TaskResult result)
        {
            switch (result)
            {
                case TaskResult.Failed:
                    Console.WriteLine("##vso[task.complete result=Failed;]FAIL");
                    break;
                case TaskResult.SucceededWithIssues:
                    Console.WriteLine("##vso[task.complete result=SucceededWithIssues;]DONE WITH ISSUES");
                    break;
                case TaskResult.Succeeded:
                    Console.WriteLine("##vso[task.complete result=Succeeded;]DONE");
                    break;
            }
        }

        /// <inheritdoc />
        public override void Log(LType logType, string valueString)
        {
            lock (_lock)
            {
                switch (logType)
                {
                    case LType.Exception:
                        oneOrMoreErrorsLogged = true;
                        Console.WriteLine($"##[error]{valueString}");
                        break;
                    case LType.Error:
                        oneOrMoreErrorsLogged = true; 
                        Console.WriteLine($"##[error]{valueString}");
                        break;
                    case LType.Warning:
                        oneOrMoreWarningsLogged = true;
                        Console.WriteLine($"##[warning]{valueString}");
                        break;
                    case LType.Info:
                        Console.WriteLine(valueString);
                        break;
                    case LType.Debug:
                        Console.WriteLine($"##[debug]{valueString}");
                        break;
                    case LType.Command:
                        Console.WriteLine($"##[command]{valueString}");
                        break;
                }
            }
        }
    }
}
