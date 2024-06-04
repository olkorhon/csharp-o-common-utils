using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Olli.Common.Logging.UnitTests
{
    [TestClass]
    public class LogEntryTests
    {
        [TestMethod]
        [DataRow(LType.Command  , "", "")]
        [DataRow(LType.Debug    , "", "")]
        [DataRow(LType.Error    , "", "")]
        [DataRow(LType.Exception, "", "")]
        [DataRow(LType.Info     , "", "")]
        [DataRow(LType.Warning  , "", "")]
        [DataRow(LType.Command,
                 "Actual command message",
                 "Actual command message")]
        [DataRow(LType.Debug,
                 "Actual debug message",
                 "Actual debug message")]
        [DataRow(LType.Error,
                 "Actual error message",
                 "Actual error message")]
        [DataRow(LType.Exception,
                 "Actual exception message",
                 "Actual exception message")]
        [DataRow(LType.Info,
                 "Actual info message",
                 "Actual info message")]
        [DataRow(LType.Warning,
                 "Actual warning message",
                 "Actual warning message")]
        public void Constructor(LType lType, string msg, string expectedMsg)
        {
            LogEntry testEntry = new LogEntry(lType, msg);
            Assert.AreEqual(testEntry.LogType, lType);
            Assert.AreEqual(testEntry.Msg, expectedMsg,
                "Stored LogEntry does not match provided input");
        }

        [TestMethod]
        [DataRow(LType.Command)]
        [DataRow(LType.Debug)]
        [DataRow(LType.Error)]
        [DataRow(LType.Exception)]
        [DataRow(LType.Info)]
        [DataRow(LType.Warning)]
        public void Constructor_NullMsg(LType lType)
        {
            LogEntry testEntry = new LogEntry(lType, null);
            Assert.AreEqual(testEntry.LogType, lType);
            Assert.AreEqual(testEntry.Msg, "",
                "Null entries should default to empty string");
        }
    }
}