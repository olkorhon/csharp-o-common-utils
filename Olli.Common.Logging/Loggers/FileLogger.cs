using System;
using System.IO;
using System.Threading;

using Olli.Common.Buffering;

namespace Olli.Common.Logging
{
    /// <summary>
    /// Logs received messages directly to a file
    /// </summary>
    public class FileLogger : ALogger
    {
        private readonly object _lock = new object();

        private readonly string filePath;

        private readonly RingBuffer<string> msgBuffer;
        private readonly Thread writerThread;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;

            msgBuffer = new RingBuffer<string>(2048);

            writerThread = new Thread(new ThreadStart(WriterWorkerThread));
            writerThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriterWorkerThread()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            while (true)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        int messagesToWrite = msgBuffer.BufferedElementCount;
                        for (int i = 0; i < messagesToWrite; i++)
                        {
                            if (msgBuffer.BufferedElementCount <= 0)
                                break;

                            string msgToWrite = msgBuffer.Read();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="msgString"></param>
        public override void Log(LType logType, string msgString)
        {
            lock (_lock)
            {
                switch (logType)
                {
                    case LType.Exception:
                        oneOrMoreErrorsLogged = true;
                        msgBuffer.Buffer(msgString);
                        break;
                    case LType.Error:
                        oneOrMoreErrorsLogged = true;
                        msgBuffer.Buffer($"[Error] {msgString}");
                        break;
                    case LType.Warning:
                        oneOrMoreWarningsLogged = true;
                        msgBuffer.Buffer($"[Warning] {msgString}");
                        break;
                    case LType.Info:
                        msgBuffer.Buffer(msgString);
                        break;
                    case LType.Debug:
                        msgBuffer.Buffer($"[Debug] {msgString}");
                        break;
                    case LType.Command:
                        msgBuffer.Buffer($"[Command] {msgString}");
                        break;
                }
            }
        }
    }
}
