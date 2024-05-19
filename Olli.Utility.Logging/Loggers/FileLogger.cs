using Ponsse.Olli.CommieTester.Utility;
using System;
using System.IO;
using System.Threading;

namespace Olli.Utility.Logging
{
    /// <summary>
    /// Logs received messages directly to a file
    /// </summary>
    public class FileLogger : ALogger
    {
        private object _lock = new object();

        string filePath;
        StreamWriter writer;

        Thread writerThread;
        StringBuffer msgBuffer;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
            writer = null;

            writerThread = new Thread(new ThreadStart(WriterWorkerThread));
            msgBuffer = new StringBuffer(2048);

            writerThread.Start();
        }

        private void WriterWorkerThread()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            while (true)
            {
                try
                {
                    using (writer = new StreamWriter(filePath, true))
                    {
                        int messagesToWrite = msgBuffer.BufferLoadMessageCount;
                        for (int i = 0; i < messagesToWrite; i++)
                        {
                            if (msgBuffer.BufferLoadMessageCount <= 0)
                                break;

                            string msgToWrite = msgBuffer.GetBufferedMsg();
                            writer.WriteLine(msgToWrite);
                            writer.Flush();
                        }
                    }
                }
                // Something went wrong, tough luck
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wrong: {e.Message}");
                }
                
                // Wait a second before writing buffered messages
                Thread.Sleep(1000);
            }
        }

        public override void Log(LType logType, string valueString)
        {
            lock (_lock)
            {
                switch (logType)
                {
                    case LType.Exception:
                        oneOrMoreErrorsLogged = true;
                        msgBuffer.BufferMsg(valueString);
                        break;
                    case LType.Error:
                        oneOrMoreErrorsLogged = true;
                        msgBuffer.BufferMsg($"[Error] {valueString}");
                        break;
                    case LType.Warning:
                        oneOrMoreWarningsLogged = true;
                        msgBuffer.BufferMsg($"[Warning] {valueString}");
                        break;
                    case LType.Info:
                        msgBuffer.BufferMsg(valueString);
                        break;
                    case LType.Debug:
                        msgBuffer.BufferMsg($"[Debug] {valueString}");
                        break;
                    case LType.Command:
                        msgBuffer.BufferMsg($"[Command] {valueString}");
                        break;
                }
            }
        }
    }
}
