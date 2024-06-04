using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Olli.Common.Logging.AzureTfsLogger;

namespace Olli.Common.Logging.UnitTests
{
    [TestClass]
    public class LoggerTests
    {
        AzureTfsLogger? logger;

        StringWriter? outWriter;
        StringWriter? errWriter;

        [TestInitialize]
        public void Init()
        {
            logger = new AzureTfsLogger();

            outWriter = new StringWriter();
            errWriter = new StringWriter();

            Console.SetOut(outWriter);
            Console.SetError(errWriter);
        }

        [TestMethod]
        [DataRow(IssueType.Error, "", "##vso[task.logissue type=error;]")]
        [DataRow(IssueType.Error, "Actual error issue", "##vso[task.logissue type=error;]Actual error issue")]
        [DataRow(IssueType.Warning, "", "##vso[task.logissue type=warning;]")]
        [DataRow(IssueType.Warning, "Actual warning issue", "##vso[task.logissue type=warning;]Actual warning issue")]
        public void AzureTfsLogger_LogIssue(IssueType issueType, string msg, string expectedMsg)
        {
            Assert.IsNotNull(outWriter, "Failure in test init, outWriter is null");
            Assert.IsNotNull(errWriter, "Failure in test init, errWriter is null");
            Assert.IsNotNull(logger   , "Failure in test init, logger is null");

            logger.LogIssue(issueType, msg);

            string outText = outWriter.ToString().TrimEnd('\r', '\n');
            string errText = errWriter.ToString().TrimEnd('\r', '\n');

            Assert.AreEqual(expectedMsg, outText);
            Assert.AreEqual("", errText);
        }

        [TestMethod]
        [DataRow(TaskResult.Failed, "##vso[task.complete result=Failed;]FAIL")]
        [DataRow(TaskResult.Succeeded, "##vso[task.complete result=Succeeded;]DONE")]
        [DataRow(TaskResult.SucceededWithIssues, "##vso[task.complete result=SucceededWithIssues;]DONE WITH ISSUES")]
        public void AzureTfsLogger_WriteTaskResult(TaskResult result, string expectedMsg)
        {
            Assert.IsNotNull(outWriter, "Failure in test init, outWriter is null");
            Assert.IsNotNull(errWriter, "Failure in test init, errWriter is null");
            Assert.IsNotNull(logger   , "Failure in test init, logger is null");

            logger.WriteTaskResult(result);

            string outText = outWriter.ToString().TrimEnd('\r', '\n');
            string errText = errWriter.ToString().TrimEnd('\r', '\n');

            Assert.AreEqual(expectedMsg, outText);
            Assert.AreEqual("", errText);
        }
    }
}
